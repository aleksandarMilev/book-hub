import ReviewForm from "../review-form/ReviewForm";

export default function CreateReview({
  bookId,
  refreshReviews,
  setIsReviewCreated,
}) {
  return (
    <ReviewForm
      bookId={bookId}
      refreshReviews={refreshReviews}
      setIsReviewCreatedOrEdited={setIsReviewCreated}
    />
  );
}
