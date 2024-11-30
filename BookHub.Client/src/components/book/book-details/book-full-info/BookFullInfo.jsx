import { useContext } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { FaEdit, FaTrash } from 'react-icons/fa'
import { format } from 'date-fns'

import * as bookApi from '../../../../api/bookApi'
import renderStars from '../../../../common/functions/renderStars'
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
                        <h5 className="book-author mb-3 text-muted">by {book.authorName || 'Unknown author'}</h5>
                        <div className="d-flex align-items-center mb-3">
                            {renderStars(book.averageRating)}
                            <span className="ms-2 text-muted">({book.ratingsCount} {book.ratingsCount === 1 ? 'review' : 'reviews'})</span>
                        </div>
                        <div className="genres mb-3">
                            <span className="fw-semibold text-muted me-1">Genres:</span>
                            {book.genres.map(g => (
                                <Link to={routes.genres + `/${g.id}`}>
                                    <span key={g.id} className="badge bg-secondary me-1">{g.name}</span>
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
                            {showFullDescription ? "Show Less" : "Show More"}
                        </button>
                        <p className="book-published-date text-muted mt-3">
                            Published: {formattedDate}
                        </p>
                        <div className="d-flex gap-2 mt-4">
                            {isCreator && (
                                <>
                                    <Link
                                        to={`${routes.editBook}/${id}`}
                                        className="btn btn-warning d-flex align-items-center gap-2"
                                    >
                                        <FaEdit /> Edit
                                    </Link>
                                    <a href="#" className="btn btn-danger d-flex align-items-center gap-2" onClick={deleteHandler}>
                                        <FaTrash /> Delete
                                    </a>
                                </>
                            )}
                            {(isAdmin && !book.isApproved) && (
                                <>
                                   <a href="#" className="btn btn-success d-flex align-items-center gap-2" onClick={approveHandler}>
                                        <FaTrash /> Approve
                                    </a>
                                    <a href="#" className="btn btn-danger d-flex align-items-center gap-2" onClick={rejectHandler}>
                                        <FaTrash /> Reject
                                    </a>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
