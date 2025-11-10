import type { GenreDetails, GenreName } from '@/features/genre/types/genre';
import { createBaseApi } from '@/shared/api/baseApiBuilder';
import { httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<GenreName, GenreDetails, null>()
  .withGetClient(httpClient.get)
  .withRoutes(routes.genres)
  .withErrors(errors.genre)
  .build();

export default api;
