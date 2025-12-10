import { useFormik } from 'formik';
import type React from 'react';
import { useState } from 'react';

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
  const isEditMode = !!existingReview;
  const [rating, setRating] = useState<number>(existingReview?.rating || 0);

  const createReview = useCreate();
  const editReview = useEdit();

  const formik = useFormik<{
    content: string;
    rating: number;
    submit?: string;
  }>({
    initialValues: {
      content: existingReview?.content || '',
      rating,
      submit: '',
    },
    enableReinitialize: true,
    validationSchema: reviewSchema,
    onSubmit: async (values, { setErrors, resetForm, setSubmitting }) => {
      try {
        const payload: CreateReview = { ...values, bookId };

        if (isEditMode) {
          const success = await editReview(existingReview!.id, payload);
          if (success) {
            setIsReviewCreatedOrEdited(true);

            await Promise.resolve(refreshReviews());

            resetForm({ values: { content: '', rating: 0, submit: '' } });
            setRating(0);
          }
        } else {
          const newId = await createReview(payload);
          if (newId) {
            setIsReviewCreatedOrEdited(true);

            await Promise.resolve(refreshReviews());

            resetForm({ values: { content: '', rating: 0, submit: '' } });
            setRating(0);
          }
        }
      } catch {
        setErrors({ submit: 'Error submitting review. Please try again.' });
      } finally {
        setSubmitting(false);
      }
    },
  });

  const handleRating = (value: number) => {
    setRating(value);
    formik.setFieldValue('rating', value);
  };

  return { formik, rating, handleRating, isEditMode };
};
