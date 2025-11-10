import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import { createBaseApi } from '@/shared/api/baseApiBuilder';
import { httpAdminClient, httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

const api = createBaseApi<null, ArticleDetails, CreateArticle>()
  .withGetClient(httpClient.get)
  .withPostClient(httpAdminClient.post)
  .withPutClient(httpAdminClient.put)
  .withDeleteClient(httpAdminClient.delete)
  .withRoutes(routes.article)
  .withErrors(errors.article)
  .build();

export default api;
