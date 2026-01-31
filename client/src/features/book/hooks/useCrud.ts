import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/book/api/api';
import type { Book, BookDetails, CreateBook } from '@/features/book/types/book';
import { routes } from '@/shared/lib/constants/api';
import { pagination } from '@/shared/lib/constants/defaultValues';
import { errors } from '@/shared/lib/constants/errorMessages';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';
import { HttpError } from '@/shared/types/errors/httpError';

export function useTopThree() {
  const { t } = useTranslation('home');
  const [books, setBooks] = useState<Book[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    (async () => {
      try {
        setIsFetching(true);
        setError(null);

        const data = await api.topThree(controller.signal);
        setBooks(data);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : errors.book.topThree;
        setError(message);
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, []);

  return { t, books, isFetching, error };
}

export function useByGenre(
  genreId: string | number,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  enabled: boolean,
) {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [books, setBooks] = useState<Book[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token || !genreId || !enabled) {
      return;
    }

    const controller = new AbortController();
    (async () => {
      try {
        setIsFetching(true);

        const result = await api.byGenre(token, String(genreId), page, pageSize, controller.signal);
        setBooks(result.items);
        setTotalItems(result.totalItems);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : errors.book.all;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [genreId, page, pageSize, token, navigate, enabled]);

  return { books, totalItems, isFetching };
}

export function useByAuthor(
  authorId: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  enabled: boolean,
) {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [books, setBooks] = useState<Book[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token || !authorId || !enabled) {
      return;
    }

    const controller = new AbortController();

    (async () => {
      try {
        setIsFetching(true);

        const result = await api.byAuthor(
          token,
          String(authorId),
          page,
          pageSize,
          controller.signal,
        );

        setBooks(result.items);
        setTotalItems(result.totalItems);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : errors.book.all;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [authorId, page, pageSize, token, navigate, enabled]);

  return { books, totalItems, isFetching };
}

export function useFullInfo(id?: string) {
  const { token, isAdmin } = useAuth();

  const [book, setBook] = useState<BookDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  const fetchData = useCallback(async () => {
    if (!id) {
      setError(
        HttpError.with()
          .message(errors.book.byId)
          .and()
          .name('Book Error')
          .and()
          .status(HttpStatusCode.NotFound)
          .create(),
      );

      return;
    }

    const controller = new AbortController();

    try {
      setIsFetching(true);
      const data = await api.details(id, token, isAdmin, controller.signal);
      setBook(data);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      setError(error as HttpError);
    } finally {
      setIsFetching(false);
      controller.abort();
    }
  }, [id, token, isAdmin]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { book, isFetching, error, refreshBook: fetchData };
}

export const useCreate = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (bookData: CreateBook) => {
      try {
        return await api.create(bookData, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to create book.';
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );

  return createHandler;
};

export const useEdit = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (id: string, bookData: CreateBook) => {
      try {
        await api.edit(id, bookData, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to edit book.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
};

export const useRemove = (id?: string, title?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!id) {
      return;
    }

    if (!showModal) {
      toggleModal();
      return;
    }

    const controller = new AbortController();

    try {
      await api.remove(id, token, controller.signal);

      showMessage(`${title || 'This book'} was successfully deleted!`, true);
      navigate(routes.home);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : errors.book.delete;
      showMessage(message, false);
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [showModal, id, token, title, navigate, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};


