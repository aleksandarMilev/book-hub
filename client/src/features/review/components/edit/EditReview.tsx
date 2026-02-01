import type { Dispatch, FC, SetStateAction } from 'react';

import ReviewForm from '@/features/review/components/form/ReviewForm';
import type { Review } from '@/features/review/types/review';

type Props = {
  bookId: string;
  refreshReviews: () => void | Promise<void>;
  existingReview: Review;
  setIsReviewEdited: Dispatch<SetStateAction<boolean>>;
};

const EditReview: FC<Props> = ({ bookId, refreshReviews, existingReview, setIsReviewEdited }) => {
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


