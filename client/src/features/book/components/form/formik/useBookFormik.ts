import { useFormik } from 'formik';
import { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { useNames } from '@/features/author/hooks/useCrud';
import {
  type BookFormValues,
  bookSchema,
} from '@/features/book/components/form/validation/bookSchema';
import { useCreate, useEdit } from '@/features/book/hooks/useCrud';
import type { BookDetails } from '@/features/book/types/book';
import { useAll } from '@/features/genre/hooks/useCrud';
import type { GenreName } from '@/features/genre/types/genre';
import { routes } from '@/shared/lib/constants/api';
import { slugify } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';

const toDateInput = (value?: string | null): string => {
  if (!value) {
    return '';
  }

  return value.slice(0, 10);
};

type Props = {
  bookData?: BookDetails | null;
  isEditMode?: boolean;
};

export const useBookFormik = ({ bookData = null, isEditMode = false }: Props) => {
  const { t } = useTranslation('books');
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

  const initialValues = useMemo<BookFormValues>(
    () => ({
      title: bookData?.title ?? '',
      authorId: bookData?.author?.id ?? null,
      image: null,
      publishedDate: toDateInput(bookData?.publishedDate),
      shortDescription: bookData?.shortDescription ?? '',
      longDescription: bookData?.longDescription ?? '',
      genres: bookData?.genres?.map((g) => g.id) ?? [],
    }),
    [bookData],
  );

  const formik = useFormik<BookFormValues>({
    initialValues,
    enableReinitialize: true,
    validationSchema: bookSchema,
    onSubmit: async (values, { setSubmitting, resetForm }) => {
      const normalizeDate = (value: unknown): string | null | undefined => {
        if (!value) {
          return null;
        }

        if (value instanceof Date && !isNaN(value.getTime())) {
          return value.toISOString().split('T')[0];
        }

        if (typeof value === 'string' && value.trim() !== '') {
          return value;
        }

        return null;
      };

      const payload = {
        title: values.title,
        authorId: values.authorId || null,
        image: values.image ?? null,
        shortDescription: values.shortDescription,
        longDescription: values.longDescription,
        publishedDate: normalizeDate(values.publishedDate),
        genres: values.genres,
      };

      try {
        if (isEditMode && bookData?.id) {
          await editHandler(bookData.id, payload);

          const titleForMessage = bookData.title || t('form.fallbackTitle');

          showMessage(t('form.messages.updateSuccess', { title: titleForMessage }), true);
          navigate(`${routes.book}/${bookData.id}`);
        } else {
          const created = await createHandler(payload);

          if (created) {
            const nameForMessage = payload.title || t('form.fallbackName');
            const messageKey = isAdmin
              ? 'form.messages.createSuccessAdmin'
              : 'form.messages.createSuccessUser';

            showMessage(t(messageKey, { name: nameForMessage }), true);
            navigate(`${routes.book}/${created.id}/${slugify(payload.title)}`, { replace: true });
            resetForm();
          }
        }
      } catch {
        const fallback = t('form.messages.operationFailed');
        showMessage(fallback, false);
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
