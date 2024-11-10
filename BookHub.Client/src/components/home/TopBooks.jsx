import React from 'react'
import { Link } from 'react-router-dom'
import { MDBCard, MDBCardImage, MDBCardBody, MDBCardTitle, MDBCardText, MDBCardGroup, MDBBtn } from 'mdb-react-ui-kit'
import { FaStar, FaRegStar, FaBook } from 'react-icons/fa'

import { routes } from '../../common/constants/api'

export default function TopBooks() {
    const books = [
        {
            title: 'Pet Sematary',
            author: 'Stephen King',
            shortDescription: 'A terrifying novel from the King of horror.',
            rating: 4.67,
            image: 'https://m.media-amazon.com/images/I/91ndIrptO4L._AC_UF1000,1000_QL80_.jpg'
        },
        {
            title: 'The Silmarillion',
            author: 'J.R.R. Tolkien',
            shortDescription: 'Introduction to the world of Lord of the Rings.',
            rating: 4.12,
            image: 'https://g.christianbook.com/dg/product/cbd/f400/338012.jpg'
        },
        {
            title: 'Harry Potter and the Deathly Hallows',
            author: 'J.K. Rowling',
            shortDescription: 'The boy who lived versus the One whose name should not be pronounced.',
            rating: 4.81,
            image: 'https://m.media-amazon.com/images/I/81aCMT1zKtL._AC_UF1000,1000_QL80_.jpg'
        }
    ]

    const renderStars = (rating) => {
        const filledStars = Math.round(rating)
        const totalStars = 5

        return (
            <div className="d-flex justify-content-center align-items-center">
                <span style={{ fontSize: '1rem', color: '#FFA500', marginRight: '4px' }}>
                    {rating.toFixed(2)}
                </span>
                {[...Array(filledStars)].map((_, i) => (
                    <FaStar key={i} color="gold" />
                ))}
                {[...Array(totalStars - filledStars)].map((_, i) => (
                    <FaRegStar key={i + filledStars} color="grey" />
                ))}
            </div>
        )
    }

    return (
        <div style={{ textAlign: 'center', padding: '2rem', fontFamily: "'Roboto', sans-serif" }}>
            <h2 className="text-center mb-4" style={{ fontFamily: "'Merriweather', serif", fontWeight: '700' }}>Top Three Books</h2>
            <MDBCardGroup style={{ justifyContent: 'center' }}>
                {books.map((book, index) => (
                    <MDBCard key={index} style={{ maxWidth: '20rem', margin: '1rem', borderRadius: '10px' }}>
                        <MDBCardImage 
                            src={book.image} 
                            alt={`${book.title} cover`} 
                            position="top" 
                            style={{ height: '300px', objectFit: 'cover', borderTopLeftRadius: '10px', borderTopRightRadius: '10px' }} 
                        />
                        <MDBCardBody>
                            <MDBCardTitle style={{ fontSize: '1.2rem', fontWeight: 'bold' }}>
                                <FaBook style={{ marginRight: '8px', color: '#FF6347' }} />
                                {book.title}
                            </MDBCardTitle>
                            <MDBCardText>
                                <strong>By:</strong> {book.author}
                            </MDBCardText>
                            <MDBCardText>{book.shortDescription}</MDBCardText>
                            <MDBCardText>{renderStars(book.rating)}</MDBCardText>
                            <MDBBtn tag={Link} to={routes.books} color='dark' rounded size='sm'>
                                View
                            </MDBBtn>
                        </MDBCardBody>
                    </MDBCard>
                ))}
            </MDBCardGroup>
        </div>
    )
}
