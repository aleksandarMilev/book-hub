import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import baseApi from '@/shared/api/baseApiBuilder';
import { httpAdminClient, httpClient } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export default baseApi<null, ArticleDetails, CreateArticle>()
  .with()
  .getClient(httpClient.get)
  .and()
  .postClient(httpAdminClient.post)
  .and()
  .putClient(httpAdminClient.put)
  .and()
  .deleteClient(httpAdminClient.delete)
  .and()
  .routes(routes.article)
  .and()
  .errors(errors.article)
  .create();
