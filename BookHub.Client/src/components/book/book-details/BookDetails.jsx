import { useContext, useState, useRef, useEffect } from "react"
import { useParams, useNavigate, Link } from "react-router-dom"

import * as bookApi from '../../../api/bookApi'
import * as useBook from '../../../hooks/useBook'
import { routes } from "../../../common/constants/api"
import { useMessage } from "../../../contexts/messageContext"
import { UserContext } from "../../../contexts/userContext"

import BookFullInfo from './book-full-info/BookFullInfo'
import AuthorIntroduction from './author-introduction/AuthorIntroduction'
import CreateReview from "./create-review/CreateReview"
import EditReview from "./edit-review/EditReview"
import ReviewItem from './review-item/ReviewItem'
import DeleteModal from '../../common/delete-modal/DeleteModal'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './BookDetails.css'

export default function BookDetails() {
    const { id } = useParams()
    const navigate = useNavigate()
    const firstReviewRef = useRef(null)
    const [showModal, setShowModal] = useState(false) 
    const [showFullDescription, setShowFullDescription] = useState(false)

    const { userId, token, hasProfile, isAdmin } = useContext(UserContext)
    const { book, isFetching, refreshBook } = useBook.useGetFullInfo(id)

    const { showMessage } = useMessage()

    const [isReviewCreated, setIsReviewCreated] = useState(false)
    const [isReviewEdited, setIsReviewEdited] = useState(false)

    const toggleModal = () => setShowModal(prev => !prev)

    const deleteHandler = async () => {
        if (showModal) {
            const success = await bookApi.deleteAsync(id, token)
            
            if(success){
                showMessage(`${book?.title || 'This book'} was successfully deleted!`, true)
                navigate(routes.book)
            } else {
                showMessage(`Somehting went wrong while deleting ${book?.title || 'this book'}, please, try again!`, false)
            }
        } else {
            toggleModal()  
        }
    }

    useEffect(() => {
        if ((isReviewCreated || isReviewEdited) && firstReviewRef.current) {
            firstReviewRef.current.scrollIntoView({ behavior: 'smooth' })

            setIsReviewCreated(false)
            setIsReviewEdited(false)
        }
    }, [isReviewCreated, isReviewEdited, book?.reviews])

    const refreshReviews = async () => {
        await refreshBook()
    }

    if(isFetching || !book){
        return(
            <div className="spinner-container d-flex justify-content-center align-items-center">
                <DefaultSpinner />
            </div>
        )
    } 

    const creatorId = book ? book.creatorId : null
    const isCreator = userId === creatorId

    const previewTextLength = 200
    const descriptionPreview = book?.longDescription?.slice(0, previewTextLength)

    const existingReview = book.reviews?.find(review => review.creatorId === userId)
    
    return (
            <div className="book-details-container mt-5">
                <div className="book-details-card shadow-lg p-4">
                    <BookFullInfo
                        book={book}
                        descriptionPreview={descriptionPreview}
                        showFullDescription={showFullDescription}
                        setShowFullDescription={setShowFullDescription}
                        isCreator={isCreator}
                        deleteHandler={deleteHandler}
                        refreshBook={refreshBook}
                        id={id}
                    />
                    <AuthorIntroduction author={book.author} />
                    {!isAdmin && hasProfile 
                        ? (
                            <>
                                {!existingReview && (
                                    <CreateReview 
                                        bookId={id}
                                        refreshReviews={refreshBook}
                                        setIsReviewCreated={setIsReviewCreated} 
                                    />
                                )}
                                {existingReview && (
                                    <EditReview 
                                        bookId={id}
                                        existingReview={existingReview}
                                        setIsReviewEdited={setIsReviewEdited} 
                                        refreshReviews={refreshBook}
                                    />
                                )}
                                </>
                        ) : (
                            !isAdmin && <>
                                <h3 className="create-review-heading">Create Review</h3>
                                <div className="create-profile-container">
                                    <Link 
                                        className="create-profile-link"
                                        to={routes.profile}
                                    >
                                        Create Profile
                                    </Link>
                                </div>
                            </>
                        )}
                   <div className="reviews-section mt-4 text-center">
                        <h5 className="reviews-title">Reviews</h5>
                        {book.reviews && book.reviews.length > 0 ? (
                            book.reviews.map((r, index) => (
                                <div
                                    ref={index === 0 ? firstReviewRef : null}
                                    key={r.id}
                                >
                                    <ReviewItem review={r}  onVote={refreshReviews}/>
                                </div>
                            ))
                            ) : (
                                <p className="no-reviews-message">No reviews yet.</p>
                        )}
                        {book.moreThanFiveReviews && 
                        <Link
                            className='reviews-button'
                            to={`${routes.review}/${book.id}`}
                            state={book.title}
                        >
                            All Reviews
                        </Link>}
                    </div>
                </div>
                    <DeleteModal 
                        showModal={showModal} 
                        toggleModal={toggleModal} 
                        deleteHandler={deleteHandler} 
                    />
            </div>
        )   
}
