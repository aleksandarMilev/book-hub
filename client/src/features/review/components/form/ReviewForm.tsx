import './ReviewForm.css';

import { MDBBtn, MDBIcon, MDBTextArea } from 'mdb-react-ui-kit';
import type React from 'react';
import type { Dispatch, FC, SetStateAction } from 'react';

import { useReviewFormik } from '@/features/review/components/form/formik/useReviewFormik.js';
import type { Review } from '@/features/review/types/review.js';

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
  const { formik, rating, handleRating, isEditMode } = useReviewFormik({
    bookId,
    refreshReviews,
    setIsReviewCreatedOrEdited,
    existingReview,
  });

  return (
    <div className="review-form-container">
      <form className="review-form" onSubmit={formik.handleSubmit}>
        <h4 className="text-center mb-4">{isEditMode ? 'Edit Review' : 'Leave a Review'}</h4>
        <div className="rating-container mb-3" role="radiogroup" aria-label="Select rating">
          {[1, 2, 3, 4, 5].map((value) => (
            <MDBIcon
              key={value}
              icon="star"
              role="radio"
              aria-checked={value === rating}
              tabIndex={0}
              className={`rating-star ${value <= rating ? 'active' : ''}`}
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
          <div className="error-text">{formik.errors.rating as string}</div>
        )}
        <MDBTextArea
          wrapperClass="mb-3"
          id="content"
          rows={6}
          label="Write your review *"
          {...formik.getFieldProps('content')}
          className={formik.touched.content && formik.errors.content ? 'is-invalid' : ''}
        />
        {formik.touched.content && formik.errors.content && (
          <div className="error-text">{formik.errors.content as string}</div>
        )}
        {formik.errors.submit && (
          <div className="error-text mb-2">{formik.errors.submit as string}</div>
        )}
        <MDBBtn type="submit" className="mb-4" block disabled={formik.isSubmitting}>
          {isEditMode
            ? formik.isSubmitting
              ? 'Saving...'
              : 'Update Review'
            : formik.isSubmitting
              ? 'Submitting...'
              : 'Submit Review'}
        </MDBBtn>
      </form>
    </div>
  );
};

export default ReviewForm;
