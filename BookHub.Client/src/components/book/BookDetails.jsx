import { useContext, useState } from "react"
import { useParams, useNavigate, Link } from "react-router-dom"
import { FaEdit, FaTrash } from 'react-icons/fa'

import renderStars from '../../common/functions/renderStars'
import * as bookApi from '../../api/bookApi'
import * as useBook from '../../hooks/useBook'
import { routes } from "../../common/constants/api"
import { UserContext } from "../../contexts/userContext"

import DefaultSpinner from '../common/DefaultSpinner'

export default function BookDetails() {
    const { id } = useParams()
    const { userId, token } = useContext(UserContext)
    const { book, isFetching } = useBook.useGetDetails(id)
    const [showFullDescription, setShowFullDescription] = useState(false)
    const navigate = useNavigate()

    async function deleteHandler(){
        await bookApi.deleteAsync(id, token)
        navigate(routes.books)
    }

    const bookUserId = book ? book.userId : null
    const isCreator = userId === bookUserId

    const previewTextLength = 200
    const descriptionPreview = book?.longDescription?.slice(0, previewTextLength)

    return (
        !isFetching ? (
          book ? (
            <div className="container mt-5">
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
                        <span className="ms-2 text-muted">
                          ({book.ratingsCount} ratings)
                        </span>
                      </div>
                      <div className="mb-3">
                        <span className="fw-semibold text-muted me-1">Genres:</span>
                        {book && book.genres.map((genre, index) => (
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
            </div>
          ) : (
            <div className="container mt-5">
              <div className="alert alert-danger text-center" role="alert">
                <h4 className="alert-heading">Oops!</h4>
                <p>The book you are looking for was not found.</p>
              </div>
            </div>
          )
        ) : (
          <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '50vh' }}>
            <DefaultSpinner />
          </div>
        )
    )
}
