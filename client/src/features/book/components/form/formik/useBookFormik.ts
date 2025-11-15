import { useFormik } from 'formik';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { useNames } from '@/features/author/hooks/useCrud';
import { useCreate, useEdit } from '@/features/book/hooks/useCrud';
import type { BookDetails } from '@/features/book/types/book';
import { useAll } from '@/features/genre/hooks/useCrud';
import type { GenreName } from '@/features/genre/types/genre';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';

import { bookSchema } from '../validation/bookSchema';

export interface BookFormValues {
  title: string;
  authorId: number | null;
  imageUrl: string;
  publishedDate: string;
  shortDescription: string;
  longDescription: string;
  genres: number[];
}

export const useBookFormik = ({
  bookData = null,
  isEditMode = false,
}: {
  bookData?: BookDetails | null;
  isEditMode?: boolean;
}) => {
  const { isAdmin } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const { authors, isFetching: authorsLoading } = useNames();
  const { genres, isFetching: genresLoading } = useAll();

  const [selectedGenres, setSelectedGenres] = useState<GenreName[]>([]);

  useEffect(() => {
    if (bookData?.genres) {
      setSelectedGenres(bookData.genres);
    }
  }, [bookData]);

  const formik = useFormik<BookFormValues>({
    initialValues: {
      title: bookData?.title ?? '',
      authorId: (bookData?.author?.id as number | null) ?? null,
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
