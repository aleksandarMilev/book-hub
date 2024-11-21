import ReviewForm from '../review-form/ReviewForm'

export default function CreateReview({ bookId, refreshReviews }){
    return(<ReviewForm bookId={bookId} refreshReviews={refreshReviews}/>)
}