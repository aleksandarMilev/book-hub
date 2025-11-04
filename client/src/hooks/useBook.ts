import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/book/bookApi';
import { routes } from '../common/constants/api';
import { errors } from '../common/constants/messages';
import { pagination } from '../common/constants/defaultValues';
import { UserContext } from '../contexts/user/userContext';
import type { Book } from '../api/book/types/book.type';

export function useGetTopThree() {
  const { token } = useContext(UserContext);
  const [books, setBooks] = useState<Book[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) {
      return;
    }

    const fetchData = async () => {
      try {
        setIsFetching(true);
        const data = await api.topThree(token);
        setBooks(data);
      } catch (error) {
        const message = error instanceof Error ? error.message : errors.book.topThree;
        setError(message);
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
  }, [token]);

  return { books, isFetching, error };
}

export function useByGenre(
  genreId: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [books, setBooks] = useState<Book[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token || !genreId) {
      return;
    }

    const fetchData = async () => {
      try {
        setIsFetching(true);
        const result = await api.byGenre(token, genreId, page, pageSize);
        setBooks(result.items);
        setTotalItems(result.totalItems);
      } catch (error) {
        const message = error instanceof Error ? error.message : errors.search.badRequest;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
  }, [genreId, page, pageSize, token, navigate]);

  return { books, totalItems, isFetching };
}

export function useByAuthor(
  authorId: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [books, setBooks] = useState<Book[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token || !authorId) return;

    const fetchData = async () => {
      try {
        setIsFetching(true);
        const result = await api.byAuthor(token, authorId, page, pageSize);

        setBooks(result.items);
        setTotalItems(result.totalItems);
      } catch (error) {
        const message = error instanceof Error ? error.message : errors.search.badRequest;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
  }, [authorId, page, pageSize, token, navigate]);

  return { books, totalItems, isFetching };
}

export function useGetFullInfo(id: string) {
  const { token, isAdmin } = useContext(UserContext);
  const navigate = useNavigate();

  const [book, setBook] = useState<Book | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token || !id) {
      return;
    }

    try {
      setIsFetching(true);
      const data = await api.details(id, token, isAdmin);
      setBook(data);
    } catch {
      navigate(routes.notFound, { state: { message: errors.book.notfound } });
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
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (bookData: Omit<Book, 'id'>) => {
      const bookToSend: Book = {
        ...bookData,
        imageUrl: bookData.imageUrl || null,
        authorId: bookData.authorId || null,
        authorName: bookData.authorName || null,
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
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (id: number, bookData: Book) => {
      const bookToSend: Book = {
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
