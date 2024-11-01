import { useParams } from "react-router-dom";

import useFetch from '../../hooks/useFetch'
import { getDetailsAsync } from '../../api/bookAPI';

import DefaultSpinner from '../common/DefaultSpinner'
import { useGetBookDetails } from "../../hooks/useBooks";

export default function BookDetails() {
    const { id } = useParams()
    const { book, isFetching } = useGetBookDetails(id)

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
    );
}

