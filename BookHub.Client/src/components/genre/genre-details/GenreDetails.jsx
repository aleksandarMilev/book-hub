import { useParams } from 'react-router-dom'
import { MDBTypography, MDBCardText } from 'mdb-react-ui-kit'

import * as useGenre from '../../../hooks/useGenre'

import BookListItem from '../../book/book-list-item/BooksListItem'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './GenreDetails.css';

export default function GenreDetails() {
    const { id } = useParams()
    const { genre, isFetching } = useGenre.useDetails(id)

    if (isFetching || !genre) {
        return <DefaultSpinner />
    }

    return (
        <div className="container genre-details mt-5">
            <div className="card shadow-lg p-3 mb-5 bg-white rounded">
                <div className="row g-0">
                    <div className="col-md-4 genre-image">
                        <img
                            src={genre.imageUrl}
                            alt={`${genre.name} genre`}
                            className="img-fluid rounded-start"
                        />
                    </div>
                    <div className="col-md-8">
                        <div className="card-body">
                            <h1 className="card-title text-primary">{genre.name}</h1>
                            <p className="card-text text-muted">{genre.description}</p>
                        </div>
                    </div>
                </div>
            </div>
            <div className="top-books-section mt-5">
                <MDBTypography tag="h4" className="section-title">Top Books</MDBTypography>
                {genre.topBooks && genre.topBooks.length > 0 ? (
                    genre.topBooks.map((book) => (
                        <BookListItem key={book.id} {...book} />
                    ))
                ) : (
                    <MDBCardText>No books available for this genre.</MDBCardText>
                )}
            </div>
        </div>
    )
}
