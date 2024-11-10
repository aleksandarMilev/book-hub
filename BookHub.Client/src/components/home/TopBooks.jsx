import React from 'react'
import { Link } from 'react-router-dom'
import { MDBCard, MDBCardImage, MDBCardBody, MDBCardTitle, MDBCardText, MDBCardGroup, MDBBtn } from 'mdb-react-ui-kit'
import { FaBook } from 'react-icons/fa'

import renderStars from '../../common/functions/renderStars'
import { routes } from '../../common/constants/api'

export default function TopBooks() {
    const books = [
        {
            id: 1,
            title: 'Pet Sematary',
            author: 'Stephen King',
            shortDescription: 'A terrifying novel from the King of horror.',
            rating: 4.67,
            image: 'https://m.media-amazon.com/images/I/91ndIrptO4L._AC_UF1000,1000_QL80_.jpg'
        },
        {
            id: 2,
            title: 'The Silmarillion',
            author: 'J.R.R. Tolkien',
            shortDescription: 'Introduction to the world of Lord of the Rings.',
            rating: 4.12,
            image: 'https://g.christianbook.com/dg/product/cbd/f400/338012.jpg'
        },
        {
            id: 3,
            title: 'Harry Potter and the Deathly Hallows',
            author: 'J.K. Rowling',
            shortDescription: 'The boy who lived versus the One whose name should not be pronounced.',
            rating: 4.81,
            image: 'https://m.media-amazon.com/images/I/81aCMT1zKtL._AC_UF1000,1000_QL80_.jpg'
        }
    ]

    return (
        <div style={{ textAlign: 'center', padding: '2rem', fontFamily: "'Roboto', sans-serif" }}>
            <h2 className="text-center mb-4" style={{ fontFamily: "'Merriweather', serif", fontWeight: '700' }}>Top Three Books</h2>
            <MDBCardGroup style={{ justifyContent: 'center' }}>
                {books.map(b => (
                    <MDBCard key={b.id} style={{ maxWidth: '20rem', margin: '1rem', borderRadius: '10px' }}>
                        <MDBCardImage 
                            src={b.image} 
                            alt={`${b.title} cover`} 
                            position="top" 
                            style={{ height: '300px', objectFit: 'cover', borderTopLeftRadius: '10px', borderTopRightRadius: '10px' }} 
                        />
                        <MDBCardBody>
                            <MDBCardTitle style={{ fontSize: '1.2rem', fontWeight: 'bold' }}>
                                <FaBook style={{ marginRight: '8px', color: '#FF6347' }} />
                                {b.title}
                            </MDBCardTitle>
                            <MDBCardText>
                                <strong>By:</strong> {b.author}
                            </MDBCardText>
                            <MDBCardText>{b.shortDescription}</MDBCardText>
                            <MDBCardText>{renderStars(b.rating)}</MDBCardText>
                            <MDBBtn tag={Link} to={routes.books + `/${b.id}`} color='dark' rounded size='sm'>
                                View
                            </MDBBtn>
                        </MDBCardBody>
                    </MDBCard>
                ))}
            </MDBCardGroup>
        </div>
    )
}
