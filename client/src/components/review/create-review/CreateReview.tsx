import type { FC, Dispatch, SetStateAction } from 'react';

import ReviewForm from '../review-form/ReviewForm';


const CreateReview: FC<{
  bookId: number;
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
