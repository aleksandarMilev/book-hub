import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import * as hooks from '../../../hooks/useBook';
import BookForm from '../book-form/BookForm';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import { parseId } from '../../../common/functions/utils';

const EditBook: FC = () => {
  const { id } = useParams<{ id: string }>();
  let parsedId: number | null = null;

  try {
    parsedId = parseId(id);
  } catch {}

  if (parsedId == null) {
    return <div>Invalid book id.</div>;
  }

  const { book, isFetching } = hooks.useFullInfo(parsedId);

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return <BookForm bookData={book} isEditMode />;
};

export default EditBook;
