import { type FC } from 'react';
import { useParams } from 'react-router-dom';

import * as hooks from '../../../hooks/useAuthor';
import AuthorForm from '../author-form/AuthorForm';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import { parseId } from '../../../common/functions/utils';

const EditAuthor: FC = () => {
  const { id } = useParams<{ id: string }>();
  let parsedId: number | null = null;

  try {
    parsedId = parseId(id);
  } catch {}

  if (parsedId == null) {
    return <div>Invalid author id.</div>;
  }

  const { author, isFetching } = hooks.useDetails(parsedId);

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return <AuthorForm authorData={author} isEditMode />;
};

export default EditAuthor;
