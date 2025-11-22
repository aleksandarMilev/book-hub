import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/book/api/api.js';
import type { Book, BookDetails, CreateBook } from '@/features/book/types/book.js';
import { routes } from '@/shared/lib/constants/api.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';
import { HttpError } from '@/shared/types/errors/httpError.js';
import type { IntId } from '@/shared/types/intId.js';

export function useTopThree() {
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

  return { books, isFetching, error };
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
  authorId: string | number,
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

export function useFullInfo(id: IntId | null, disable = false) {
  const { token, isAdmin } = useAuth();

  const [book, setBook] = useState<BookDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  const fetchData = useCallback(async () => {
    if (disable || !id) {
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
  }, [id, token, isAdmin, disable]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { book, isFetching, error, refreshBook: fetchData };
}

export function useCreate() {
  const { token } = useAuth();
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (bookData: CreateBook) => {
      const bookToSend: CreateBook = {
        ...bookData,
        imageUrl: bookData.imageUrl || null,
        authorId: bookData.authorId || null,
        publishedDate: bookData.publishedDate || null,
      };

      try {
        return await api.create(bookToSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.book.create;
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );

  return createHandler;
}

export function useEdit() {
  const { token } = useAuth();
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (id: number, bookData: CreateBook) => {
      const bookToSend: CreateBook = {
        ...bookData,
        imageUrl: bookData.imageUrl || null,
        authorId: bookData.authorId || null,
        publishedDate: bookData.publishedDate || null,
      };

      try {
        return await api.edit(id, bookToSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.book.edit;
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );

  return editHandler;
}

export const useRemove = (id: IntId | null, disable = false, title?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (disable || !id) {
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
  }, [showModal, id, token, title, navigate, showMessage, toggleModal, disable]);

  return { showModal, toggleModal, deleteHandler };
};
