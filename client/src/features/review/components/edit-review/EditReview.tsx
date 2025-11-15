import type { Dispatch, FC, SetStateAction } from 'react';

import ReviewForm from '@/features/review/components/review-form/ReviewForm';
import type { Review } from '@/features/review/types/review';
import type { IntId } from '@/shared/types/intId';

const EditReview: FC<{
  bookId: IntId;
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
