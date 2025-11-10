import api from '@/features/statistics/api/api';
import type { Statistics } from '@/features/statistics/types/statistics';
import baseCrudBuilder from '@/shared/hooks/useCrud/baseCrudBuilder';
import { errors } from '@/shared/lib/constants/errorMessages';

export const { useAll: useStatistics } = baseCrudBuilder<Statistics, null, null>()
  .with()
  .api(api)
  .and()
  .resource('Statistics')
  .and()
  .errors(errors.statistics)
  .create();
