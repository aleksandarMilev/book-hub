import { useCallback, useEffect, useState } from 'react';

import type { Book } from '@/features/book/types/book.js';
import * as api from '@/features/reading-list/api/api.js';
import { type ReadingStatusUI, toApiStatus } from '@/features/reading-list/types/readingList.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

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

      try {
        setIsFetching(true);
        setError(null);

        const apiStatus = toApiStatus(statusUI);
        const result = await api.get(
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
  bookId: number,
  token: string,
  showMessage: (message: string, success?: boolean) => void,
) {
  const addToList = useCallback(
    async (statusUI: ReadingStatusUI) => {
      try {
        await api.add(bookId, toApiStatus(statusUI), token);
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
      try {
        await api.remove(bookId, toApiStatus(statusUI), token);
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
