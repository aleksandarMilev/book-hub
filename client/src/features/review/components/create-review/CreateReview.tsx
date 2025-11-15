import type { Dispatch, FC, SetStateAction } from 'react';

import ReviewForm from '@/features/review/components/review-form/ReviewForm';
import type { IntId } from '@/shared/types/intId';

const CreateReview: FC<{
  bookId: IntId;
  refreshReviews: () => void | Promise<void>;
  setIsReviewCreated: Dispatch<SetStateAction<boolean>>;
}> = ({ bookId, refreshReviews, setIsReviewCreated }) => {
  return (
    <ReviewForm
      bookId={bookId}
      refreshReviews={refreshReviews}
      setIsReviewCreatedOrEdited={setIsReviewCreated}
    />
  );
};

export default CreateReview;
