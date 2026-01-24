import { useFormik } from 'formik';
import type React from 'react';
import { useTranslation } from 'react-i18next';

import { useCreate, useEdit } from '@/features/review/hooks/useCrud.js';
import type { CreateReview, Review } from '@/features/review/types/review.js';

import { reviewSchema } from '../validation/reviewSchema.js';

type Props = {
  bookId: string;
  refreshReviews: () => void | Promise<void>;
  setIsReviewCreatedOrEdited: React.Dispatch<React.SetStateAction<boolean>>;
  existingReview?: Review | null;
};

export const useReviewFormik = ({
  bookId,
  refreshReviews,
  setIsReviewCreatedOrEdited,
  existingReview = null,
}: Props) => {
  const { t } = useTranslation('reviews');

  const isEditMode = !!existingReview;

  const createReview = useCreate();
  const editReview = useEdit();

  const formik = useFormik<{
    content: string;
    rating: number;
    submit?: string;
  }>({
    initialValues: {
      content: existingReview?.content || '',
      rating: existingReview?.rating || 0,
      submit: '',
    },
    enableReinitialize: true,
    validationSchema: reviewSchema(t),
    onSubmit: async (values, { setErrors, resetForm, setSubmitting }) => {
      try {
        const payload: CreateReview = { ...values, bookId };

        if (isEditMode) {
          const success = await editReview(existingReview!.id, payload);
          if (success) {
            setIsReviewCreatedOrEdited(true);

            await Promise.resolve(refreshReviews());

            resetForm({ values: { content: '', rating: 0, submit: '' } });
          }
        } else {
          const newId = await createReview(payload);
          if (newId) {
            setIsReviewCreatedOrEdited(true);

            await Promise.resolve(refreshReviews());

            resetForm({ values: { content: '', rating: 0, submit: '' } });
          }
        }
      } catch {
        setErrors({ submit: t('form.errors.submit') });
      } finally {
        setSubmitting(false);
      }
    },
  });

  const handleRating = (value: number) => {
    formik.setFieldValue('rating', value);
  };

  return { formik, handleRating, isEditMode };
};
