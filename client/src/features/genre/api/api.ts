import type { GenreDetails, GenreName } from '@/features/genre/types/genre';
import baseApi from '@/shared/api/baseApiBuilder';
import { httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';

export default baseApi<GenreName, GenreDetails, null>()
  .with()
  .getClient(httpClient.get)
  .and()
  .routes(routes.genres)
  .create();
