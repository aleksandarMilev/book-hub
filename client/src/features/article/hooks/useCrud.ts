import api from '@/features/article/api/api';
import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import baseCrudBuilder from '@/shared/hooks/useCrud/baseCrudBuilder';
import { errors } from '@/shared/lib/constants/errorMessages';

export const { useDetails, useCreate, useEdit, useRemove } = baseCrudBuilder<
  null,
  ArticleDetails,
  CreateArticle
>()
  .with()
  .api(api)
  .and()
  .resource('Article')
  .and()
  .errors(errors.article)
  .create();
