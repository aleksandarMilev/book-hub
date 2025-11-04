import { useCallback, useEffect, useState } from 'react';
import * as api from '../api/statistics/statisticsApi';
import type { Statistics } from '../api/statistics/types/statistics';

export function useStatistics() {
  const [statistics, setStatistics] = useState<Statistics | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    try {
      setIsFetching(true);
      const result = await api.all();

      setStatistics(result);
      setError(null);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load statistics.';
      setError(message);
    } finally {
      setIsFetching(false);
    }
  }, []);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { statistics, isFetching, error, refetch: fetchData };
}
