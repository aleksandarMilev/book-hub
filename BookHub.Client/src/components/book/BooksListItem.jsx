import { Link } from 'react-router-dom'
import { FaBook, FaUser, FaTag } from 'react-icons/fa' 

import renderStars from '../../common/functions/renderStars'
import { routes } from '../../common/constants/api'

export default function BookListItem({ id, imageUrl, title, authorName, shortDescription, averageRating, genres }) {
    return (
        <div className="row p-3 bg-light border rounded mb-3 shadow-sm" style={{ fontSize: '1rem', backgroundColor: '#f8f9fa' }}>
            <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
                <img className="img-fluid img-responsive rounded" 
                     src={imageUrl} 
                     alt={title} 
                     style={{ maxHeight: '200px', width: 'auto', objectFit: 'cover' }} />
            </div>
            <div className="col-md-6 col-8 mt-1" style={{ backgroundColor: '#f8f9fa' }}>
                <h5 className="mb-2" style={{ fontSize: '1.2rem' }}>
                    <FaBook className="me-2" />{title}
                </h5>
                <h6 className="text-muted mb-2" style={{ fontSize: '1rem' }}>
                    <FaUser className="me-2" />By {authorName}
                </h6>
                <div className="d-flex flex-row mb-2" style={{ fontSize: '1rem' }}>
                    {renderStars(averageRating)}
                </div>
                <div className="mt-1 mb-2" style={{ fontSize: '1.1em' }}>
                    <FaTag className="me-2" />
                    {genres.map((genre, index) => (
                        <span key={index} className="badge bg-secondary me-1">{genre}</span>
                    ))}
                </div>
                <p className="text-justify para mb-0" style={{
                    wordWrap: 'break-word',
                    overflowWrap: 'break-word',
                    whiteSpace: 'normal', 
                    maxHeight: '100px', 
                    overflow: 'hidden', 
                    textOverflow: 'ellipsis',
                    fontSize: '1rem',
                }}>
                    {shortDescription}
                </p>
            </div>

            <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
                <div className="d-flex flex-column align-items-center">
                    <Link to={routes.books + `/${id}`} className="btn btn-sm btn-primary">
                        View Details
                    </Link>
                </div>
            </div>
        </div>
    )
}
