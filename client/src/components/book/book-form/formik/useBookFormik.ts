import { useFormik } from 'formik';
import { useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import type { BookFormProps, BookFormValues, NamedEntity } from '../../../../api/book/types/book';
import { routes } from '../../../../common/constants/api';
import { useMessage } from '../../../../contexts/message/messageContext';
import { UserContext } from '../../../../contexts/user/userContext';
import * as useAuthor from '../../../../hooks/useAuthor';
import * as useBook from '../../../../hooks/useBook';
import * as useGenre from '../../../../hooks/useGenre';

import { bookSchema } from '../validation/bookSchema';


export const useBookFormik = ({ bookData = null, isEditMode = false }: BookFormProps) => {
  const navigate = useNavigate();
  const { isAdmin } = useContext(UserContext);
  const { showMessage } = useMessage();

  const createHandler = useBook.useCreate();
  const editHandler = useBook.useEdit();

  const { authors, isFetching: authorsLoading } = useAuthor.useNames();
  const { genres, isFetching: genresLoading } = useGenre.useAll();

  const [selectedGenres, setSelectedGenres] = useState<NamedEntity[]>([]);

  useEffect(() => {
    if (bookData?.genres) {
      setSelectedGenres(bookData.genres);
    }
  }, [bookData]);

  const formik = useFormik<BookFormValues>({
    initialValues: {
      title: bookData?.title ?? '',
      authorId: (bookData?.authorId as number | null) ?? null,
      imageUrl: bookData?.imageUrl ?? '',
      publishedDate: bookData?.publishedDate ?? '',
      shortDescription: bookData?.shortDescription ?? '',
      longDescription: bookData?.longDescription ?? '',
      genres:
        bookData?.genres?.map((g) => g.id) ??
        (selectedGenres.length ? selectedGenres.map((g) => g.id) : []),
    },
    enableReinitialize: true,
    validationSchema: bookSchema,
    onSubmit: async (values, { setSubmitting, resetForm }) => {
      try {
        if (isEditMode && bookData?.id) {
          const ok = await editHandler(bookData.id, values);
          if (ok) {
            showMessage(`${bookData.title} was successfully edited!`, true);
            navigate(`${routes.book}/${bookData.id}`);
          }
        } else {
          const bookId = await createHandler(values);
          if (bookId) {
            showMessage(
              isAdmin
                ? 'Book successfully created!'
                : 'Thank you for being part of our community! Our admin team will process your book soon.',
              true,
            );
            navigate(isAdmin ? `${routes.book}/${bookId}` : routes.home);
            resetForm();
          }
        }
      } catch {
        showMessage('Something went wrong. Please, try again.', false);
      } finally {
        setSubmitting(false);
      }
    },
  });

  return {
    formik,
    authors,
    authorsLoading,
    genres,
    genresLoading,
    selectedGenres,
    setSelectedGenres,
  };
};
