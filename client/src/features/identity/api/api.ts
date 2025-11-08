import type { RegisterRequest } from '@/features/identity/types/identity';
import { createBaseApi, httpClient } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<null, null, RegisterRequest>(
  {
    publicPost: httpClient.post,
  },
  {
    login: routes.login,
    register: routes.register,
  },
  errors.identity,
);

export default api;
