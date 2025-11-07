import type { FC } from 'react';
import { useParams } from 'react-router-dom';

import { parseId } from '../../../common/functions/utils';
import * as hooks from '../../../hooks/useBook';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';

import BookForm from '../book-form/BookForm';


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
