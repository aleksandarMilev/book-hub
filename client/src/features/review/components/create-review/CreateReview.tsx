import type { Dispatch, FC, SetStateAction } from 'react';

import ReviewForm from '@/features/review/components/review-form/ReviewForm.js';

type Props = {
  bookId?: string | undefined;
  refreshReviews: () => void | Promise<void>;
  setIsReviewCreated: Dispatch<SetStateAction<boolean>>;
};

const CreateReview: FC<Props> = ({ bookId, refreshReviews, setIsReviewCreated }) => {
  return (
    <ReviewForm
      bookId={bookId}
      refreshReviews={refreshReviews}
      setIsReviewCreatedOrEdited={setIsReviewCreated}
    />
  );
};

export default CreateReview;
