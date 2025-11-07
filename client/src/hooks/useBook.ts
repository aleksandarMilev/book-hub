import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/book/bookApi';
import type {
  BookDetailsResponse,
  BookFormValues,
  BookListItemType,
  BookUpsertPayload,
  UseBookApprovalProps,
} from '../api/book/types/book';
import { routes } from '../common/constants/api';
import { pagination } from '../common/constants/defaultValues';
import { errors } from '../common/constants/messages';
import { useMessage } from '../contexts/message/messageContext';
import { UserContext } from '../contexts/user/userContext';


export function useTopThree() {
  const { token } = useContext(UserContext);

  const [books, setBooks] = useState<BookListItemType[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) {
      return;
    }

    const controller = new AbortController();
    (async () => {
      try {
        setIsFetching(true);
        setError(null);

        const data = await api.topThree(token, controller.signal);
        setBooks(data);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : errors.book.topThree;
        setError(message);
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [token]);

  return { books, isFetching, error };
}

export function useByGenre(
  genreId: string | number,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [books, setBooks] = useState<BookListItemType[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token || !genreId) {
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
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : errors.search.badRequest;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [genreId, page, pageSize, token, navigate]);

  return { books, totalItems, isFetching };
}

export function useByAuthor(
  authorId: string | number,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [books, setBooks] = useState<BookListItemType[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token || !authorId) {
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
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : errors.search.badRequest;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [authorId, page, pageSize, token, navigate]);

  return { books, totalItems, isFetching };
}

export function useFullInfo(id: number) {
  const navigate = useNavigate();
  const { token, isAdmin } = useContext(UserContext);

  const [book, setBook] = useState<BookDetailsResponse | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token || !id) {
      return;
    }

    const controller = new AbortController();
    try {
      setIsFetching(true);
      const data = await api.details(id, token, isAdmin, controller.signal);
      setBook(data);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      navigate(routes.notFound, { state: { message: errors.book.notFound } });
    } finally {
      setIsFetching(false);
    }
  }, [id, token, isAdmin, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { book, isFetching, refreshBook: fetchData };
}

export function useCreate() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const createHandler = useCallback(
    async (bookData: BookFormValues) => {
      const bookToSend: BookUpsertPayload = {
        ...bookData,
        imageUrl: bookData.imageUrl || null,
        authorId: bookData.authorId || null,
        publishedDate: bookData.publishedDate || null,
      };

      try {
        return await api.create(bookToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : errors.book.create;
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return createHandler;
}

export function useEdit() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const editHandler = useCallback(
    async (id: number, bookData: BookFormValues) => {
      const bookToSend: BookUpsertPayload = {
        ...bookData,
        imageUrl: bookData.imageUrl || null,
        authorId: bookData.authorId || null,
        publishedDate: bookData.publishedDate || null,
      };

      try {
        return await api.edit(id, bookToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : errors.book.edit;
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

export const useRemove = (id: number, title?: string) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (showModal) {
      try {
        await api.remove(id, token);

        showMessage(`${title || 'This book'} was successfully deleted!`, true);
        navigate(routes.book);
      } catch (error) {
        const message = error instanceof Error ? error.message : errors.book.delete;
        showMessage(message, false);
      } finally {
        toggleModal();
      }
    } else {
      toggleModal();
    }
  }, [showModal, id, token, title, navigate, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

export function useApproval({ id, token, showMessage }: UseBookApprovalProps) {
  const navigate = useNavigate();

  const approveHandler = useCallback(async () => {
    try {
      await api.approve(id, token);

      showMessage('You have successfully approved the book!', true);
      return true;
    } catch {
      showMessage('Error approving the book. Please try again!', false);
      return false;
    }
  }, [id, token, showMessage]);

  const rejectHandler = useCallback(async () => {
    try {
      await api.reject(id, token);

      showMessage('You have successfully rejected the book!', true);
      navigate(routes.home);

      return true;
    } catch {
      showMessage('Error rejecting the book. Please try again!', false);
      return false;
    }
  }, [id, token, showMessage, navigate]);

  return { approveHandler, rejectHandler };
}
