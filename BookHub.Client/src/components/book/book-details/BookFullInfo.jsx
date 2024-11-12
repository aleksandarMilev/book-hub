import React from 'react'
import { Link } from 'react-router-dom'
import { FaEdit, FaTrash } from 'react-icons/fa'

import renderStars from '../../../common/functions/renderStars'
import { routes } from "../../../common/constants/api"

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
        <div className="card shadow-lg p-4" style={{ backgroundColor: '#f9f9f9' }}>
            <div className="row g-0">
                <div className="col-md-4 d-flex align-items-center position-sticky" style={{ top: '80px' }}>
                    <img
                        src={book.imageUrl}
                        alt="Book Cover"
                        className="img-fluid rounded shadow-sm"
                        style={{ maxHeight: '300px', objectFit: 'contain', width: '100%' }}
                    />
                </div>
                <div className="col-md-8">
                    <div className="card-body">
                        <h2 className="card-title fw-bold">{book.title}</h2>
                        <h5 className="card-subtitle mb-3 text-muted">by {book.authorName}</h5>
                        <div className="d-flex align-items-center mb-3">
                            {renderStars(book.averageRating)}
                            <span className="ms-2 text-muted">({book.ratingsCount} ratings)</span>
                        </div>
                        <div className="mb-3">
                            <span className="fw-semibold text-muted me-1">Genres:</span>
                            {book.genres.map((genre, index) => (
                                <span key={index} className="badge bg-secondary me-1">{genre}</span>
                            ))}
                        </div>
                        <p className="card-text" style={{ lineHeight: '1.6' }}>
                            {showFullDescription ? book.longDescription : `${descriptionPreview}...`}
                        </p>
                        <button
                            onClick={() => setShowFullDescription(prev => !prev)}
                            className="btn btn-link p-0 text-decoration-none text-primary"
                            style={{ fontWeight: 'bold', padding: '0', fontSize: '1.1rem' }}
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
    )
}
