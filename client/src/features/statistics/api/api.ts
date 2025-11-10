import type { Statistics } from '@/features/statistics/types/statistics';
import { createBaseApi, httpClient } from '@/shared/api/baseApiBuilder';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<Statistics, null, null>(
  {
    all: httpClient.get,
  },
  routes.statistics,
  errors.statistics,
);

export default api;
