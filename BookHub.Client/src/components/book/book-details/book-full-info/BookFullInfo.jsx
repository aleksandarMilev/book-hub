import { Link } from 'react-router-dom'
import { FaEdit, FaTrash } from 'react-icons/fa'

import renderStars from '../../../../common/functions/renderStars'
import { routes } from "../../../../common/constants/api"

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
                            <span className="ms-2 text-muted">({book.ratingsCount} ratings)</span>
                        </div>
                        <div className="genres mb-3">
                            <span className="fw-semibold text-muted me-1">Genres:</span>
                            {book.genres.map((genre, index) => (
                                <span key={index} className="badge bg-secondary me-1">{genre}</span>
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
