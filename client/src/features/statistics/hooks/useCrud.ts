import api from '@/features/statistics/api/api';
import type { Statistics } from '@/features/statistics/types/statistics';
import { createCrudHooks } from '@/shared/hooks/useCrud/useCrud';
import { errors } from '@/shared/lib/constants/errorMessages';

export const { useAll: useStatistics } = createCrudHooks<Statistics, null, null>({
  api,
  resourceName: 'Statistics',
  errors: errors.statistics,
});
