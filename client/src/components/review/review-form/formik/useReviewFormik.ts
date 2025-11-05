import { useState } from 'react';
import { useFormik } from 'formik';
import * as hooks from '../../../../hooks/useReview';
import { reviewSchema } from '../validation/reviewSchema';
import type { Review, ReviewInput } from '../../../../api/review/types/review';

export const useReviewFormik = ({
  bookId,
  refreshReviews,
  setIsReviewCreatedOrEdited,
  existingReview = null,
}: {
  bookId: number;
  refreshReviews: () => void | Promise<void>;
  setIsReviewCreatedOrEdited: React.Dispatch<React.SetStateAction<boolean>>;
  existingReview?: Review | null;
}) => {
  const isEditMode = !!existingReview;
  const [rating, setRating] = useState<number>(existingReview?.rating || 0);

  const createReview = hooks.useCreateReview();
  const editReview = hooks.useEditReview();

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
        const payload: ReviewInput = { ...values, bookId };

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
