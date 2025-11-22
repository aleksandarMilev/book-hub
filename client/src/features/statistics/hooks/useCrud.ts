import { useCallback, useEffect, useState } from 'react';

import * as api from '@/features/statistics/api/api.js';
import type { Statistics } from '@/features/statistics/types/statistics.js';
import { IsError } from '@/shared/lib/utils/utils.js';

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
      const message = IsError(error) ? error.message : 'Failed to load statistics.';
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
