import { useCallback, useContext, useEffect, useState } from 'react';
import * as api from '../api/readingList/readingListApi';
import { UserContext } from '../contexts/user/userContext';
import type { ReadingListItem, ReadingStatus } from '../api/readingList/types/readingList';

export function useList(
  status: ReadingStatus,
  page: number | null = null,
  pageSize: number | null = null,
  isPrivate = false,
  userIdParam?: string,
) {
  const { token, userId } = useContext(UserContext);
  const [readingList, setReadingList] = useState<ReadingListItem[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [isFetching, setIsFetching] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) return;

      if (isPrivate && userIdParam !== userId) {
        setReadingList([]);
        setTotalItems(0);
        setError(null);

        return;
      }

      try {
        setIsFetching(true);

        const result = await api.get(userIdParam!, token, status!, page, pageSize, signal);

        setReadingList(result.items ?? []);
        setTotalItems(result.totalItems ?? 0);
        setError(null);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') return;

        const message = error instanceof Error ? error.message : 'Failed to load reading list.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    },
    [userIdParam, token, userId, status, page, pageSize, isPrivate],
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
    async (status: ReadingStatus) => {
      try {
        const result = await api.add(bookId, status!, token);

        if (result?.errorMessage) {
          showMessage(result.errorMessage, false);
          return false;
        }

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
    async (status: ReadingStatus | null) => {
      try {
        await api.remove(bookId, status!, token);

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
