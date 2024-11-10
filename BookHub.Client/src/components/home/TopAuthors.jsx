import React from 'react'
import { Link } from 'react-router-dom'
import { MDBBadge, MDBBtn, MDBTable, MDBTableHead, MDBTableBody } from 'mdb-react-ui-kit'
import { FaStar, FaRegStar } from 'react-icons/fa'

import renderStars from '../../common/functions/renderStars'
import { routes } from '../../common/constants/api'

export default function TopAuthors() {
    const authors = [
        {
            image: 'https://upload.wikimedia.org/wikipedia/commons/6/6f/Tolkien-color.png',
            name: 'J.R.R. Tolkien',
            genres: ['Fantasy'],
            totalBooks: 12,
            rating: 4.49
        },
        {
            image: 'https://hips.hearstapps.com/hmg-prod/images/gettyimages-1061157246.jpg',
            name: 'J.K. Rowling',
            genres: ['Fantasy'],
            totalBooks: 10,
            rating: 4.65
        },
        {
            image: 'https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/926_v9_bc.jpg',
            name: 'Stephen King',
            genres: ['Horror', 'Thriller', 'Sci-Fi'],
            totalBooks: 112,
            rating: 4.96
        }
    ]

    return (
        <>
            <h2 className="text-center my-5" style={{ fontFamily: 'Roboto, sans-serif', color: '#333', fontSize: '2.5rem' }}>
                Top Authors
            </h2>
            <MDBTable align="middle" className="table-striped">
                <MDBTableHead>
                    <tr 
                        style={{
                            backgroundColor: '#f8f9fa',
                            color: '#333',
                            fontWeight: 'bold',
                            fontFamily: 'Roboto, sans-serif',
                            fontSize: '1.25rem' 
                        }}
                    >
                        <th scope="col">Author</th>
                        <th scope="col">Genres</th>
                        <th scope="col">Total Books</th>
                        <th scope="col">Rating</th>
                        <th scope="col"></th>
                    </tr>
                </MDBTableHead>
                <MDBTableBody>
                    {authors.map((author, index) => (
                        <tr key={index} style={{ fontSize: '1.25rem' }}>
                            <td>
                                <div className="d-flex align-items-center">
                                    <img
                                        src={author.image}
                                        alt={author.name}
                                        style={{
                                            width: '75px',
                                            height: '75px',
                                            borderRadius: '50%',
                                            marginRight: '1rem'
                                        }}
                                    />
                                    <div>
                                        <p className="fw-bold mb-1" style={{ color: '#333' }}>
                                            {author.name}
                                        </p>
                                    </div>
                                </div>
                            </td>
                            <td>
                                {author.genres.map((genre, i) => (
                                    <MDBBadge key={i} color="primary" pill className="me-2" style={{ fontSize: '1.1rem' }}>
                                        {genre}
                                    </MDBBadge>
                                ))}
                            </td>
                            <td style={{ color: '#333' }}>{author.totalBooks}</td>
                            <td>{renderStars(author.rating)}</td>
                            <td>
                                <MDBBtn 
                                    tag={Link} 
                                    to={routes.author} 
                                    color="dark" 
                                    rounded 
                                    size="lg" 
                                >
                                    View
                                </MDBBtn>
                            </td>
                        </tr>
                    ))}
                </MDBTableBody>
            </MDBTable>
        </>
    )
}
