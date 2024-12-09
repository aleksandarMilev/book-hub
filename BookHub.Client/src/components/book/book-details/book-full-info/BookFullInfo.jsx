import { useContext, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { FaEdit, FaTrash } from 'react-icons/fa'
import { format } from 'date-fns'

import * as bookApi from '../../../../api/bookApi'
import * as readingListApi from '../../../../api/readingListApi'
import renderStars from '../../../../common/functions/renderStars'
import { readingListStatus } from '../../../../common/constants/defaultValues'
import { routes } from "../../../../common/constants/api"
import { UserContext } from '../../../../contexts/userContext'
import { useMessage } from '../../../../contexts/messageContext'

import './BookFullInfo.css'

export default function BookFullInfo({
    book,
    descriptionPreview,
    showFullDescription,
    setShowFullDescription,
    isCreator,
    deleteHandler,
    id
}) {

    const { isAdmin, token, hasProfile } = useContext(UserContext)
    const { showMessage } = useMessage()

    const formattedDate = book.publishedDate 
        ? format(new Date(book.publishedDate), 'MMMM dd, yyyy')
        : 'Publication date unknown'

        console.log(book)

    return (
        <div className="book-info-card shadow-lg p-4">
            <div className="row g-0">
                <div className="col-md-4 book-info-image-container">
                    <img
                        src={book.imageUrl}
                        alt="Book Cover"
                        className="book-info-image"
                    />
                </div>
                <div className="col-md-8">
                    <div className="card-body">
                        <h2 className="book-title fw-bold">{book.title}</h2>
                        <h5 className="book-author mb-3 text-muted">
                            by {book.authorName || 'Unknown author'}
                        </h5>
                        <div className="d-flex align-items-center mb-3">
                            {renderStars(book.averageRating)}
                            <span className="ms-2 text-muted">
                                ({book.ratingsCount}{' '}
                                {book.ratingsCount === 1 ? 'review' : 'reviews'})
                            </span>
                        </div>
                        <div className="genres mb-3">
                            <span className="fw-semibold text-muted me-1">
                                Genres:
                            </span>
                            {book.genres.map(g => (
                                <Link to={routes.genres + `/${g.id}`} key={g.id}>
                                    <span className="badge bg-secondary me-1">
                                        {g.name}
                                    </span>
                                </Link>
                            ))}
                        </div>
                        <p className="book-description card-text">
                            {showFullDescription ? book.longDescription : `${descriptionPreview}...`}
                        </p>
                        <button
                            onClick={() => setShowFullDescription(prev => !prev)}
                            className="btn btn-link p-0 text-decoration-none text-primary show-more-button"
                        >
                            {showFullDescription ? 'Show Less' : 'Show More'}
                        </button>
                        <p className="book-published-date text-muted mt-3">
                            Published: {formattedDate}
                        </p>
                        <div className="read-buttons-section mt-4">
                        <div className="read-buttons-section mt-4">
                            {isAdmin  
                                ? (
                                <ApproveRejectButtons
                                    bookId={book.id}
                                    initialIsApproved={book.isApproved}
                                    token={token}
                                    showMessage={showMessage}
                                />
                                ) : 
                                    hasProfile 
                                    ? (
                                        <ReadingListButtons
                                            bookId={book.id}
                                            initialReadingStatus={book.readingStatus}
                                            token={token}
                                            showMessage={showMessage}
                                        />
                                    ) : (
                                    <Link to={routes.profile}>Create Profile</Link>
                                )}
                        </div>

                        </div>
                        <div className="d-flex gap-2 mt-4">
                            {isCreator && (
                                <Link
                                    to={`${routes.editBook}/${id}`}
                                    className="btn btn-warning d-flex align-items-center gap-2"
                                >
                                    <FaEdit /> Edit
                                </Link>
                            )}
                            {(isCreator || isAdmin) && (
                                <button className="btn btn-danger d-flex align-items-center gap-2" onClick={deleteHandler}>
                                    <FaTrash /> Delete
                                </button>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

function ReadingListButtons({ bookId, initialReadingStatus, token, showMessage }) {
    const [readingStatus, setReadingStatus] = useState(initialReadingStatus)

    const handleAddToList = async (status) => {
        try {
            const error = await readingListApi.addInListAsync(bookId, status, token);
            if (error) {
                showMessage(error.errorMessage, false)
            } else {
                setReadingStatus(status)
                showMessage(`You have successfully updated your reading list!`, true)
            }
        } catch (error) {
            showMessage(`Error updating your reading list. Please try again!`, false)
        }
    }

    const handleRemoveFromList = async () => {
        try {
            await readingListApi.removeFromListAsync(bookId, readingStatus, token)
            setReadingStatus(null)
            showMessage(`You have successfully removed the book from your list!`, true)
        } catch (error) {
            showMessage(`Error removing the book. Please try again!`, false)
        }
    }

    if (!readingStatus) {
        return (
            <div className="d-flex gap-2">
                <button
                    className="btn btn-outline-success"
                    onClick={() => handleAddToList(readingListStatus.read)}
                >
                    Mark as Read
                </button>
                <button
                    className="btn btn-outline-primary"
                    onClick={() => handleAddToList(readingListStatus.toRead)}
                >
                    Add to Want to Read
                </button>
                <button
                    className="btn btn-outline-warning"
                    onClick={() => handleAddToList(readingListStatus.currentlyReading)}
                >
                    Add to Currently Reading
                </button>
            </div>
        )
    }

    return (
        <div className="reading-status">
            {readingStatus.toLowerCase() === readingListStatus.read.toLowerCase() && (
                <p>You marked this book as <strong>Read</strong>.</p>
            )}
            {readingStatus.toLowerCase() === readingListStatus.toRead.toLowerCase() && (
                <p>You added this book to <strong>Want to Read</strong>.</p>
            )}
            {readingStatus.toLowerCase() === readingListStatus.currentlyReading.toLowerCase() && (
                <p>You are currently <strong>Reading</strong> this book.</p>
            )}
            <button className="btn btn-outline-danger" onClick={handleRemoveFromList}>
                Remove from List
            </button>
        </div>
    )
}

function ApproveRejectButtons({ bookId, initialIsApproved, token, showMessage }) {
    const navigate = useNavigate()
    const [isApproved, setIsApproved] = useState(initialIsApproved)

    const approveHandler = async () => {
        try {
            await bookApi.approveAsync(bookId, token)
            showMessage(`You have successfully approved the book!`, true)
            setIsApproved(true)
        } catch (error) {
            showMessage(`Error approving the book. Please try again!`, false)
        }
    }

    const rejectHandler = async () => {
        try {
            await bookApi.rejectAsync(bookId, token)
            showMessage(`You have successfully rejected the book!`, true)
            navigate(routes.home)
        } catch (error) {
            showMessage(`Error rejecting the book. Please try again!`, false)
        }
    }

    return !isApproved ? (
        <div className="d-flex gap-2">
            <button className="btn btn-success d-flex align-items-center gap-2" onClick={approveHandler}>
                Approve
            </button>
            <button className="btn btn-danger d-flex align-items-center gap-2" onClick={rejectHandler}>
                Reject
            </button>
        </div>
    ) : (
        <p className="text-success">This book has been approved.</p>
    )
}
