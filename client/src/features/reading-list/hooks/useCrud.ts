import { useCallback, useEffect, useState } from 'react';

import type { Book } from '@/features/book/types/book';
import * as api from '@/features/reading-list/api/api';
import { type ReadingStatusUI, toApiStatus } from '@/features/reading-list/types/readingList';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';

export function useLastCurrentlyReading(isPrivate = false, ownerId?: string) {
  const { token, userId } = useAuth();
  const [book, setBook] = useState<Book | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      if (isPrivate && ownerId && userId && ownerId !== userId) {
        setBook(null);
        setError(null);

        return;
      }

      try {
        setIsFetching(true);
        setError(null);

        const result = await api.getLastCurrentlyReading(ownerId ?? userId!, token, signal);
        setBook(result);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load reading list.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    },
    [token, ownerId, userId, isPrivate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { book, isFetching, error };
}

export function useList(
  statusUI: ReadingStatusUI,
  page: number | null = null,
  pageSize: number | null = null,
  isPrivate = false,
  ownerId?: string,
  disabled = false,
) {
  const { token, userId } = useAuth();
  const [readingList, setReadingList] = useState<Book[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (disabled) {
        setReadingList([]);
        setTotalItems(0);
        setError(null);
        return;
      }

      if (!token) {
        return;
      }

      if (isPrivate && ownerId && userId && ownerId !== userId) {
        setReadingList([]);
        setTotalItems(0);
        setError(null);
        return;
      }

      const apiStatus = toApiStatus(statusUI);
      if (apiStatus == null) {
        setReadingList([]);
        setTotalItems(0);
        setError('Invalid reading status.');
        return;
      }

      try {
        setIsFetching(true);
        setError(null);

        const result = await api.getList(
          ownerId ?? userId!,
          token,
          apiStatus,
          page ?? undefined,
          pageSize ?? undefined,
          signal,
        );

        setReadingList(result?.items ?? []);
        setTotalItems(result?.totalItems ?? 0);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load reading list.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    },
    [disabled, token, ownerId, userId, statusUI, page, pageSize, isPrivate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { readingList, totalItems, isFetching, error, refetch: fetchData };
}

export function useListActions(
  bookId: string,
  token: string,
  showMessage: (message: string, success?: boolean) => void,
) {
  const addToList = useCallback(
    async (statusUI: ReadingStatusUI) => {
      const apiStatus = toApiStatus(statusUI);
      if (apiStatus == null) {
        showMessage('Invalid reading status.', false);
        return false;
      }

      try {
        await api.add(bookId, apiStatus, token);
        showMessage('Book added to your reading list!', true);
        return true;
      } catch {
        showMessage('Failed to update reading list.', false);
        return false;
      }
    },
    [bookId, token, showMessage],
  );

  const removeFromList = useCallback(
    async (statusUI: ReadingStatusUI) => {
      const apiStatus = toApiStatus(statusUI);
      if (apiStatus == null) {
        showMessage('Invalid reading status.', false);
        return false;
      }

      try {
        await api.remove(bookId, apiStatus, token);
        showMessage('Book removed from your reading list!', true);
        return true;
      } catch {
        showMessage('Failed to remove book from list.', false);
        return false;
      }
    },
    [bookId, token, showMessage],
  );

  return { addToList, removeFromList };
}


