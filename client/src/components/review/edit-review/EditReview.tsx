import type { FC, Dispatch, SetStateAction } from 'react';
import ReviewForm from '../review-form/ReviewForm';
import type { Review } from '../../../api/review/types/review';

const EditReview: FC<{
  bookId: number;
  refreshReviews: () => void | Promise<void>;
  existingReview: Review;
  setIsReviewEdited: Dispatch<SetStateAction<boolean>>;
}> = ({ bookId, refreshReviews, existingReview, setIsReviewEdited }) => {
  return (
    <ReviewForm
      bookId={bookId}
      refreshReviews={refreshReviews}
      setIsReviewCreatedOrEdited={setIsReviewEdited}
      existingReview={existingReview}
    />
  );
};

export default EditReview;
