import { FaBook } from 'react-icons/fa'
import { Link } from 'react-router-dom'  
import { routes } from '../../../../common/constants/api'

import './AuthorIntroduction.css'

export default function AuthorIntroduction({ author }) {
    const previewBio = author.biography.slice(0, 200)

    return (
        <div className="author-intro-card">
            <h3 className="author-intro-title">About the Author</h3>
            <div className="author-intro-header">
                <div className="author-intro-image-container">
                    <img
                        src={author.imageUrl}
                        alt={author.name}
                        className="author-intro-image"
                    />
                    <div className="author-intro-info">
                        <h4 className="author-name">{author.name}</h4>
                        <div className="author-books-count">
                            <FaBook className="author-book-icon" />
                            <p>{author.booksCount} {author.booksCount !== 1 ? 'Books' : 'Book'}</p>
                        </div>
                    </div>
                </div>
            </div>
            <div className="author-intro-bio">
                <p className="author-bio-text">
                    {previewBio}
                    <span className="see-more">...</span>
                    <Link 
                        to={routes.author + `/${author.id}`} 
                        className="see-more-link"
                    >
                        See More
                    </Link>
                </p>
            </div>
        </div>
    )
}
