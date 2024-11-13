import { Link } from 'react-router-dom'
import { MDBBadge, MDBBtn, MDBTable, MDBTableHead, MDBTableBody } from 'mdb-react-ui-kit'

import renderStars from '../../../common/functions/renderStars'
import { routes } from '../../../common/constants/api'

import './TopAuthors.css'  

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
            <h2 className="text-center my-5 top-authors-title">
                Top Authors
            </h2>
            <MDBTable align="middle" className="table-striped">
                <MDBTableHead>
                    <tr className="table-header">
                        <th scope="col">Author</th>
                        <th scope="col">Genres</th>
                        <th scope="col">Total Books</th>
                        <th scope="col">Rating</th>
                        <th scope="col"></th>
                    </tr>
                </MDBTableHead>
                <MDBTableBody>
                    {authors.map((author, index) => (
                        <tr key={index} className="author-row">
                            <td>
                                <div className="d-flex align-items-center">
                                    <img
                                        src={author.image}
                                        alt={author.name}
                                        className="author-image"
                                    />
                                    <div>
                                        <p className="author-name mb-1">
                                            {author.name}
                                        </p>
                                    </div>
                                </div>
                            </td>
                            <td>
                                {author.genres.map((genre, i) => (
                                    <MDBBadge key={i} color="primary" pill className="me-2 genre-badge">
                                        {genre}
                                    </MDBBadge>
                                ))}
                            </td>
                            <td>{author.totalBooks}</td>
                            <td>{renderStars(author.rating)}</td>
                            <td>
                                <MDBBtn 
                                    tag={Link} 
                                    to={routes.author} 
                                    color="dark" 
                                    rounded 
                                    size="lg" 
                                    className="view-button"
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
