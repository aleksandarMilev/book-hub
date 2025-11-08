import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import ArticleForm from '@/features/article/components/form/ArticleForm';
import * as hooks from '@/features/article/hooks/useArticle';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';
import { toIntId } from '@/shared/lib/utils';

const EditArticle: FC = () => {
  const { id } = useParams<{ id: string }>();

  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { article, isFetching, error } = hooks.useDetails(parsedId, disable);

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !article) {
    return <DefaultSpinner />;
  }

  return <ArticleForm article={article} isEditMode />;
};

export default EditArticle;
