import { Link } from 'react-router-dom'
import { FaBook, FaUser, FaTag } from 'react-icons/fa'

import renderStars from '../../../common/functions/renderStars'
import { routes } from '../../../common/constants/api'

import './BookListItem.css' 

export default function BookListItem({ id, imageUrl, title, authorName, shortDescription, averageRating, genres }) {
    console.log(genres);
    
    return (
        <div className="row p-3 bg-light border rounded mb-3 shadow-sm book-list-item">
            <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
                <img className="img-fluid img-responsive rounded book-list-item-image" 
                     src={imageUrl} 
                     alt={title} />
            </div>
            <div className="col-md-6 col-8 mt-1 book-list-item-content">
                <h5 className="mb-2 book-list-item-title">
                    <FaBook className="me-2" />{title}
                </h5>
                <h6 className="text-muted mb-2 book-list-item-author">
                    <FaUser className="me-2" />By {authorName || 'Unknown Author'}
                </h6>
                <div className="d-flex flex-row mb-2 book-list-item-rating">
                    {renderStars(averageRating)}
                </div>
                <div className="mt-1 mb-2 book-list-item-genres">
                    <FaTag className="me-2" />
                    {genres.map(g => (
                        <span key={g.id} className="badge bg-secondary me-1">{g.name}</span>
                    ))}
                </div>
                <p className="text-justify para mb-0 book-list-item-description">
                    {shortDescription}
                </p>
            </div>

            <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
                <div className="d-flex flex-column align-items-center">
                    <Link to={routes.books + `/${id}`} className="btn btn-sm btn-primary book-list-item-btn">
                        View Details
                    </Link>
                </div>
            </div>
        </div>
    )
}
