import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import * as hooks from '../../../hooks/useArticle';
import ArticleForm from '../article-form/ArticleForm';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';

const EditArticle: FC = () => {
  const { id } = useParams<{ id: string }>();
  const { article, isFetching } = hooks.useDetails(id ?? '');

  if (isFetching || !article) {
    return <DefaultSpinner />;
  }

  return <ArticleForm article={article} isEditMode={true} />;
};

export default EditArticle;
