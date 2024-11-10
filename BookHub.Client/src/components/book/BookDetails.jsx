import { useContext } from "react"
import { useParams, useNavigate, Link } from "react-router-dom"

import * as bookApi from '../../api/bookApi'
import * as useBook from '../../hooks/useBook'
import { routes } from "../../common/constants/api"
import { UserContext } from "../../contexts/userContext"

import DefaultSpinner from '../common/DefaultSpinner'

export default function BookDetails() {
    const { id } = useParams()
    const { userId, token } = useContext(UserContext)
    const { book, isFetching } = useBook.useGetDetails(id)
    const navigate = useNavigate()

    async function deleteHandler(){
        await bookApi.deleteAsync(id, token)
        navigate(routes.books)
    }

    const bookUserId = book ? book.userId : null
    const isCreator = userId === bookUserId

    return (
        !isFetching ? (
            book ? (
                <div className="container mt-5">
                <div className="card shadow-lg p-4">
                    <div className="row g-0">
                        <div className="col-md-4 d-flex align-items-center">
                            <img
                                src={book.imageUrl}
                                alt="Book Cover"
                                className="img-fluid rounded shadow-sm"
                                style={{ maxHeight: '300px', objectFit: 'contain', width: '100%' }}
                            />
                        </div>
                        <div className="col-md-8">
                            <div className="card-body">
                                <h2 className="card-title fw-bold text-primary">{book.title}</h2>
                                <h5 className="card-subtitle mb-3 text-muted">by {book.author}</h5>
                                <p className="card-text text-secondary" style={{ lineHeight: '1.6' }}>
                                    {book.description}
                                </p>
                                <div className="d-flex gap-2 mt-4">
                                    <a href="#" className="btn btn-primary">Buy Now</a>
                                    <a href="#" className="btn btn-outline-primary">Add to Wishlist</a>
                                    {isCreator && (
                                    <>
                                        <Link 
                                            to={routes.editBook + `/${id}`}
                                            className="btn btn-warning ms-2">
                                                Edit
                                        </Link>
                                        <a href="#" className="btn btn-danger ms-2" onClick={deleteHandler}>
                                            Delete
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
