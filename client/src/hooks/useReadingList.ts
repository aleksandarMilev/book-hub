import { useCallback, useContext, useEffect, useState } from 'react';

import * as api from '../api/readingList/readingListApi';
import { UserContext } from '../contexts/user/userContext';
import type { ReadingListItem } from '../api/readingList/types/readingListItem';

export function useReadingList(
  id: string,
  status: string,
  page: number | null = null,
  pageSize: number | null = null,
  isPrivate = false,
) {
  const { token, userId } = useContext(UserContext);
  const [readingList, setReadingList] = useState<ReadingListItem[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    if (!token) return;

    if (isPrivate && id !== userId) {
      setReadingList([]);
      setTotalItems(0);
      setError(null);

      return;
    }

    try {
      setIsFetching(true);

      const result = await api.get(id, token, status, page, pageSize);

      setReadingList(result.items);
      setTotalItems(result.totalItems);
      setError(null);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load reading list.';
      setError(message);
    } finally {
      setIsFetching(false);
    }
  }, [id, token, userId, status, page, pageSize, isPrivate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { readingList, totalItems, isFetching, error, refetch: fetchData };
}
