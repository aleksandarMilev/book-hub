import './ReviewForm.css';

import { MDBIcon, MDBTextArea } from 'mdb-react-ui-kit';
import type React from 'react';
import type { Dispatch, FC, SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';

import { useReviewFormik } from '@/features/review/components/form/formik/useReviewFormik';
import type { Review } from '@/features/review/types/review';

import { POSSIBLE_RATINGS } from '../../utils/constants.js';

type Props = {
  bookId: string;
  refreshReviews: () => void | Promise<void>;
  setIsReviewCreatedOrEdited: Dispatch<SetStateAction<boolean>>;
  existingReview?: Review | null;
};

const ReviewForm: FC<Props> = ({
  bookId,
  refreshReviews,
  setIsReviewCreatedOrEdited,
  existingReview = null,
}) => {
  const { t } = useTranslation('reviews');

  const { formik, handleRating, isEditMode } = useReviewFormik({
    bookId,
    refreshReviews,
    setIsReviewCreatedOrEdited,
    existingReview,
  });

  return (
    <div className="review-form-container">
      <form className="review-form" onSubmit={formik.handleSubmit}>
        <h4 className="review-form__title">
          {isEditMode ? t('form.titleEdit') : t('form.titleCreate')}
        </h4>

        <div
          className="review-form__rating"
          role="radiogroup"
          aria-label={t('form.ratingAriaLabel')}
        >
          {POSSIBLE_RATINGS.map((value) => (
            <MDBIcon
              key={value}
              icon="star"
              role="radio"
              aria-checked={value === formik.values.rating}
              tabIndex={0}
              className={`review-form__star ${value <= formik.values.rating ? 'review-form__star--active' : ''}`}
              onClick={() => handleRating(value)}
              onKeyDown={(e: React.KeyboardEvent) => {
                if (e.key === 'Enter' || e.key === ' ') {
                  handleRating(value);
                }
              }}
            />
          ))}
        </div>

        {formik.touched.rating && formik.errors.rating && (
          <div className="review-form__error">{formik.errors.rating as string}</div>
        )}

        <div className="review-form__field">
          <MDBTextArea
            wrapperClass=""
            id="content"
            rows={6}
            label={t('form.contentLabel')}
            {...formik.getFieldProps('content')}
            className={formik.touched.content && formik.errors.content ? 'is-invalid' : ''}
          />
        </div>

        {formik.touched.content && formik.errors.content && (
          <div className="review-form__error">{formik.errors.content as string}</div>
        )}

        {formik.errors.submit && (
          <div className="review-form__error review-form__error--submit">
            {formik.errors.submit as string}
          </div>
        )}

        <button type="submit" className="review-form__submit" disabled={formik.isSubmitting}>
          {isEditMode
            ? formik.isSubmitting
              ? t('form.buttons.submittingEdit')
              : t('form.buttons.submitEdit')
            : formik.isSubmitting
              ? t('form.buttons.submittingCreate')
              : t('form.buttons.submitCreate')}
        </button>
      </form>
    </div>
  );
};

export default ReviewForm;


