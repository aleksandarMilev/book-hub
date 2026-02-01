import type { FC } from 'react';

import AuthorForm from '@/features/author/components/form/AuthorForm.jsx';
import { useEditPage } from '@/features/author/hooks/useEditPage';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.jsx';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.jsx';

const EditAuthor: FC = () => {
  const { author, isFetching, error } = useEditPage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !author) {
    return <DefaultSpinner />;
  }

  return <AuthorForm authorData={author} isEditMode />;
};

export default EditAuthor;


