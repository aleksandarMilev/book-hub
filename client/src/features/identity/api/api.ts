import type { RegisterRequest } from '@/features/identity/types/identity';
import { baseApi } from '@/shared/api/baseApiBuilder';
import { httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = baseApi<null, null, RegisterRequest>()
  .publicPostClient(httpClient.post)
  .routes({
    login: routes.login,
    register: routes.register,
  })
  .errors(errors.identity)
  .create();

export default api;
