import { type FC } from 'react';
import { useParams } from 'react-router-dom';

import * as hooks from '../../../hooks/useAuthor';
import AuthorForm from '../author-form/AuthorForm';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';

const EditAuthor: FC = () => {
  const { id } = useParams<{ id: string }>();
  const { author, isFetching } = hooks.useDetails(id!);

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return <AuthorForm authorData={author} isEditMode />;
};

export default EditAuthor;
