import type { FC } from 'react';

import BookForm from '@/features/book/components/form/BookForm.js';
import { useFullInfo } from '@/features/book/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.js';
import { useParams } from 'react-router-dom';

const EditBook: FC = () => {
  const { id } = useParams<{ id: string }>();
  const { book, isFetching, error } = useFullInfo(id);

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !book) {
    return <DefaultSpinner />;
  }

  return <BookForm bookData={book} isEditMode />;
};

export default EditBook;
