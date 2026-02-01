import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import BookForm from '@/features/book/components/form/BookForm';
import { useFullInfo } from '@/features/book/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';

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


