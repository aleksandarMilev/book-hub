import ReviewForm from "../review-form/ReviewForm";

export default function EditReview({
  bookId,
  refreshReviews,
  existingReview,
  setIsReviewEdited,
}) {
  return (
    <ReviewForm
      bookId={bookId}
      refreshReviews={refreshReviews}
      setIsReviewCreatedOrEdited={setIsReviewEdited}
      existingReview={existingReview}
    />
  );
}
