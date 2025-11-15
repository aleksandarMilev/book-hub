import './BookListItem.css';

import { type FC } from 'react';
import { FaBook, FaTag, FaUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { GenreName } from '@/features/genre/types/genre';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { routes } from '@/shared/lib/constants/api';

const BookListItem: FC<{
  id: number;
  imageUrl: string;
  title: string;
  authorName: string;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
}> = ({ id, imageUrl, title, authorName, shortDescription, averageRating = 0, genres }) => {
  return (
    <div className="row p-3 bg-light border rounded mb-3 shadow-sm book-list-item">
      <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
        {imageUrl ? (
          <img
            className="img-fluid img-responsive rounded book-list-item-image"
            src={imageUrl}
            alt={title}
          />
        ) : (
          <div className="text-muted text-center">No Image</div>
        )}
      </div>
      <div className="col-md-6 col-8 mt-1 book-list-item-content">
        <h5 className="mb-2 book-list-item-title">
          <FaBook className="me-2" />
          {title}
        </h5>
        <h6 className="text-muted mb-2 book-list-item-author">
          <FaUser className="me-2" />
          By {authorName || 'Unknown Author'}
        </h6>
        <div className="d-flex flex-row mb-2 book-list-item-rating">
          <RenderStars rating={averageRating} />
        </div>
        <div className="mt-1 mb-2 book-list-item-genres">
          <FaTag className="me-2" />
          {genres.length > 0 ? (
            genres.map((g) => (
              <Link key={g.id} to={`${routes.genres}/${g.id}`}>
                <span className="badge bg-secondary me-1">{g.name}</span>
              </Link>
            ))
          ) : (
            <span className="text-muted">No genres</span>
          )}
        </div>
        {shortDescription && (
          <p className="text-justify para mb-0 book-list-item-description">{shortDescription}</p>
        )}
      </div>
      <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
        <div className="d-flex flex-column align-items-center">
          <Link to={`${routes.book}/${id}`} className="btn btn-sm btn-primary book-list-item-btn">
            View Details
          </Link>
        </div>
      </div>
    </div>
  );
};

export default BookListItem;
