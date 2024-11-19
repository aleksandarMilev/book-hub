import { Link } from 'react-router-dom'
import { MDBCard, MDBCardImage, MDBCardBody, MDBCardTitle, MDBCardText, MDBCardGroup, MDBBtn } from 'mdb-react-ui-kit'
import { FaBook, FaBookReader } from 'react-icons/fa'

import * as useBooks from '../../../hooks/useBook'
import renderStars from '../../../common/functions/renderStars'
import { routes } from '../../../common/constants/api'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './TopBooks.css' 

export default function TopBooks() {
    const { books, isFetching, error } = useBooks.useGetTopThree() 

    if (isFetching) {
        return (<DefaultSpinner />)
    }

    if (error) {
        return (
            <div className="d-flex flex-column align-items-center justify-content-center vh-50">
                <div className="text-center">
                    <FaBookReader size={100} color="red" className="mb-3" />
                    <p className="lead">{error}</p> 
                </div>
            </div>
        )
    }

    return (
        <div className="top-books-container">
            <h2 className="top-books-title mb-4">Top Three Books</h2>
            <MDBCardGroup className="card-group">
                {books.map(b => (
                    <MDBCard key={b.id} className="top-book-card">
                        <MDBCardImage 
                            src={b.imageUrl} 
                            alt={`${b.title} cover`} 
                            position="top" 
                            className="book-image" 
                        />
                        <MDBCardBody>
                            <MDBCardTitle className="card-title">
                                <FaBook className="book-icon" />
                                {b.title}
                            </MDBCardTitle>
                            <MDBCardText>
                                <strong>By:</strong> {b.authorName}
                            </MDBCardText>
                            <MDBCardText>{b.shortDescription}</MDBCardText>
                            <MDBCardText>{renderStars(b.averageRating)}</MDBCardText>
                            <MDBCardText>
                                <strong>Genres:</strong> 
                                {b.genres && b.genres.length > 0 ? (
                                    b.genres.map((g, i) => (
                                        <span key={g.id} className="genre-item">
                                            {g.name}{i < b.genres.length - 1 ? ', ' : ''}
                                        </span>
                                    ))
                                ) : (
                                    <span>No genres available</span>
                                )}
                            </MDBCardText>
                            <MDBBtn tag={Link} to={routes.books + `/${b.id}`} color='dark' rounded size='sm' className="view-button">
                                View
                            </MDBBtn>
                        </MDBCardBody>
                    </MDBCard>
                ))}
            </MDBCardGroup>
        </div>
    )
}
