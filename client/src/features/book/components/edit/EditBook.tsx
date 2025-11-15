import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import BookForm from '@/features/book/components/form/BookForm.js';
import { useFullInfo } from '@/features/book/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.js';
import { toIntId } from '@/shared/lib/utils.js';

const EditBook: FC = () => {
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { book, isFetching, error } = useFullInfo(parsedId, disable);

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !book) {
    return <DefaultSpinner />;
  }

  return <BookForm bookData={book} isEditMode />;
};

export default EditBook;
