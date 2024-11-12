import { FaBook } from 'react-icons/fa';
import { Link } from 'react-router-dom';  
import { routes } from '../../../common/constants/api';

export default function AuthorIntroduction({ author }) {
    const previewBio = author.biography.slice(0, 200); 

    return (
        <div className="card shadow-lg p-4 mt-4" style={{ backgroundColor: '#f0f0f0' }}>
            <h3 className="fw-bold text-center mb-4" style={{ fontFamily: 'Poppins, sans-serif', fontSize: '1.8rem', color: '#333' }}>About the Author</h3>
            <div className="d-flex align-items-center justify-content-between mb-4">
                <div className="d-flex align-items-center">
                    <img
                        src={author.imageUrl}
                        alt={author.name}
                        className="img-fluid rounded-circle shadow-sm"
                        style={{
                            maxWidth: '120px',
                            objectFit: 'cover',
                            marginRight: '20px',
                            border: '3px solid #ddd',
                        }}
                    />
                    <div>
                        <h4 className="fw-semibold text-dark">{author.name}</h4>
                        <div className="d-flex align-items-center text-muted">
                            <FaBook className="me-2" />
                            <p>{author.booksCount} {author.booksCount !== 1 ? 'Books' : 'Book'}</p>
                        </div>
                    </div>
                </div>
            </div>
            <div className="d-flex align-items-center" style={{
                position: 'relative',
                maxHeight: '200px',  
                overflow: 'hidden',
                paddingBottom: '20px',
                flexDirection: 'column', 
            }}>
                <p 
                    className="text-muted mb-0" 
                    style={{
                        lineHeight: '1.7',
                        position: 'relative', 
                        zIndex: 1,
                        maxWidth: 'calc(100% - 120px)', 
                    }}
                >
                    {previewBio}
                    <span style={{ display: 'inline', color: '#007bff', fontWeight: 'bold' }}>...</span>
                    <Link 
                        to={routes.author + `/${author.id}`} 
                        className="text-decoration-none text-primary ms-2" 
                        style={{
                            fontWeight: 'bold',
                            display: 'inline', 
                        }}
                    >
                        See More
                    </Link>
                </p>
            </div>
        </div>
    )
}
