import type { GenreDetails, GenreName } from '@/features/genre/types/genre';
import { createBaseApi, httpClient } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<GenreName, GenreDetails, null>(
  {
    all: httpClient.get,
    byId: httpClient.get,
  },
  routes.genres,
  errors.genre,
);

export default api;
