import type { Dispatch, FC, SetStateAction } from 'react';

import ReviewForm from '@/features/review/components/form/ReviewForm';

type Props = {
  bookId: string;
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


