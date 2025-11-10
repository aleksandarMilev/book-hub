import type { RegisterRequest } from '@/features/identity/types/identity';
import { createBaseApi } from '@/shared/api/baseApiBuilder';
import { httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<null, null, RegisterRequest>()
  .withPublicPostClient(httpClient.post)
  .withRoutes({
    login: routes.login,
    register: routes.register,
  })
  .withErrors(errors.identity)
  .build();

export default api;
