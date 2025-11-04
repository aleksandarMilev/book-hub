import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import * as hooks from '../../../hooks/useArticle';
import ArticleForm from '../article-form/ArticleForm';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import { parseId } from '../../../common/functions/utils';

const EditArticle: FC = () => {
  const { id } = useParams<{ id: string }>();
  let parsedId: number | null = null;

  try {
    parsedId = parseId(id);
  } catch {}
  if (parsedId == null) {
    return <div>Invalid article id.</div>;
  }

  const { article, isFetching, error } = hooks.useDetails(parsedId);

  if (error) {
    return <div className="alert alert-danger">{error}</div>;
  }

  if (isFetching || !article) {
    return <DefaultSpinner />;
  }

  return <ArticleForm article={article} isEditMode />;
};

export default EditArticle;
