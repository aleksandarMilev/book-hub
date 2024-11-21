import ReviewForm from '../review-form/ReviewForm'

export default function EditReview({ bookId, refreshReviews, existingReview  }){
    return(
        <ReviewForm 
            bookId={bookId}
            refreshReviews={refreshReviews}
            existingReview={existingReview}
        />
    )
}