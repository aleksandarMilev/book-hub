import { useContext } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { FaBook, FaBookmark, FaClock, FaEdit, FaTrash } from 'react-icons/fa'
import { format } from 'date-fns'

import * as bookApi from '../../../../api/bookApi'
import * as readingListApi from '../../../../api/readingListApi'
import renderStars from '../../../../common/functions/renderStars'
import { readingListStatus } from '../../../../common/constants/defaultValues'
import { routes } from "../../../../common/constants/api"
import { UserContext } from '../../../../contexts/userContext'

import './BookFullInfo.css'

export default function BookFullInfo({
    book,
    descriptionPreview,
    showFullDescription,
    setShowFullDescription,
    isCreator,
    deleteHandler,
    refreshBook,
    id
}) {

    const navigate = useNavigate()
    const { isAdmin, token } = useContext(UserContext)

    const formattedDate = book.publishedDate 
        ? format(new Date(book.publishedDate), 'MMMM dd, yyyy')
        : 'Publication date unknown'

    const approveHandler = async () => {
        try {
            await bookApi.approveAsync(book.id, token)
            refreshBook()
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    const rejectHandler = async () => {
        try {
            await bookApi.rejectAsync(book.id, token)
            navigate(routes.home)
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    const handleAddToList = async (status) => {
        try {
            await readingListApi.addInListAsync(book.id, status, token)
            refreshBook()
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    const handleRemoveFromList = async (status) => {
        try {
            await readingListApi.removeFromListAsync(book.id, status, token)
            refreshBook()
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

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
                            {book.genres.map((g) => (
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
                            onClick={() => setShowFullDescription((prev) => !prev)}
                            className="btn btn-link p-0 text-decoration-none text-primary show-more-button"
                        >
                            {showFullDescription ? 'Show Less' : 'Show More'}
                        </button>
                        <p className="book-published-date text-muted mt-3">
                            Published: {formattedDate}
                        </p>
                        <div className="read-buttons-section mt-4">
                            <h5>Manage Your Reading List:</h5>
                            {book.readingStatus === null ? (
                                <div className="d-flex gap-2 mt-2">
                                    <button
                                        className="btn btn-outline-success d-flex align-items-center gap-2"
                                        onClick={() => handleAddToList(readingListStatus.read)}
                                    >
                                        <FaBook /> Read
                                    </button>
                                    <button
                                        className="btn btn-outline-primary d-flex align-items-center gap-2"
                                        onClick={() => handleAddToList(readingListStatus.toRead)}
                                    >
                                        <FaBookmark /> Want To Read
                                    </button>
                                    <button
                                        className="btn btn-outline-warning d-flex align-items-center gap-2"
                                        onClick={() => handleAddToList(readingListStatus.currentlyReading)}
                                    >
                                        <FaClock /> Currently Reading
                                    </button>
                                </div>
                            ) : (
                                <div className={`reading-status-message mt-3 ${book.readingStatus.toLowerCase()}`}>
                                    {book.readingStatus.toLowerCase() === readingListStatus.read.toLowerCase() && (
                                        <>
                                            <p className="text-success">
                                                You have marked this book as <strong>Read</strong>.
                                            </p>
                                            <button
                                                className="btn btn-outline-danger d-flex align-items-center gap-2"
                                                onClick={() => handleRemoveFromList(readingListStatus.read)}
                                            >
                                                Remove from your list
                                            </button>
                                        </>
                                    )}
                                    {book.readingStatus.toLowerCase() === readingListStatus.toRead.toLowerCase() && (
                                        <>
                                            <p className="text-success">
                                                You have that you <strong>Want To Read</strong> this book.
                                            </p>
                                            <button
                                                className="btn btn-outline-danger d-flex align-items-center gap-2"
                                                onClick={() => handleRemoveFromList(readingListStatus.toRead)}
                                            >
                                                Remove from your list
                                            </button>
                                        </>
                                    )}
                                    {book.readingStatus.toLowerCase() === readingListStatus.currentlyReading.toLowerCase() && (
                                        <>
                                            <p className="text-success">
                                                You are currently <strong>Reading</strong> this book.
                                            </p>
                                            <button
                                                className="btn btn-outline-danger d-flex align-items-center gap-2"
                                                onClick={() => handleRemoveFromList(readingListStatus.currentlyReading)}
                                            >
                                                Remove from your list
                                            </button>
                                        </>
                                    )}
                                </div>
                            )}
                        </div>
                        <div className="d-flex gap-2 mt-4">
                            {isCreator && (
                                <>
                                    <Link
                                        to={`${routes.editBook}/${id}`}
                                        className="btn btn-warning d-flex align-items-center gap-2"
                                    >
                                        <FaEdit /> Edit
                                    </Link>
                                    <button className="btn btn-danger d-flex align-items-center gap-2" onClick={deleteHandler}>
                                        <FaTrash /> Delete
                                    </button>
                                </>
                            )}
                            {(isAdmin && !book.isApproved) && (
                                <>
                                    <button
                                        className="btn btn-success d-flex align-items-center gap-2"
                                        onClick={approveHandler}
                                    >
                                        Approve
                                    </button>
                                    <button
                                        className="btn btn-danger d-flex align-items-center gap-2"
                                        onClick={rejectHandler}
                                    >
                                        Reject
                                    </button>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
