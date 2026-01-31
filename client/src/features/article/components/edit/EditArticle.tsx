import type { FC } from 'react';

import ArticleForm from '@/features/article/components/form/ArticleForm.jsx';
import { useEditArticlePage } from '@/features/article/hooks/useEditArticlePage';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.jsx';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.jsx';

const EditArticle: FC = () => {
  const { article, isFetching, error } = useEditArticlePage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !article) {
    return <DefaultSpinner />;
  }

  return <ArticleForm article={article} isEditMode />;
};

export default EditArticle;


