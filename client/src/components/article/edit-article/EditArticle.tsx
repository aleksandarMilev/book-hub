import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import { parseId } from '../../../common/functions/utils';
import * as hooks from '../../../hooks/useArticle';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';

import ArticleForm from '../article-form/ArticleForm';


const EditArticle: FC = () => {
  const { id } = useParams<{ id: string }>();
  let parsedId = parseId(id);

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
