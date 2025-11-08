import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import { createBaseApi, httpAdminClient, httpClient } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<null, ArticleDetails, CreateArticle>(
  {
    byId: httpClient.get,
    post: httpAdminClient.post,
    put: httpAdminClient.put,
    delete: httpAdminClient.delete,
  },
  routes.article,
  errors.article,
);

export default api;
