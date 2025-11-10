import type { Statistics } from '@/features/statistics/types/statistics';
import baseApi from '@/shared/api/baseApiBuilder';
import { httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export default baseApi<Statistics, null, null>()
  .with()
  .getClient(httpClient.get)
  .and()
  .routes(routes.statistics)
  .and()
  .errors(errors.statistics)
  .create();
