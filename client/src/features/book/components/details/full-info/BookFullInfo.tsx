import './BookFullInfo.css';

import type React from 'react';
import type { FC } from 'react';
import { FaEdit, FaTrash } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { BookDetails } from '@/features/book/types/book';
import { toUiStatus } from '@/features/reading-list/types/readingList';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { routes } from '@/shared/lib/constants/api';
import { formatIsoDate } from '@/shared/lib/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';
import type { IntId } from '@/shared/types/intId';

import { ApproveRejectButtons } from './approve-reject-buttons/ApproveRejectButtons';
import { ReadingListButtons } from './reading-list-buttons/ReadingListButtons';

const BookFullInfo: FC<{
  book: BookDetails;
  descriptionPreview: string;
  showFullDescription: boolean;
  setShowFullDescription: React.Dispatch<React.SetStateAction<boolean>>;
  isCreator: boolean;
  deleteHandler: () => void;
  id: IntId;
}> = ({
  book,
  descriptionPreview,
  showFullDescription,
  setShowFullDescription,
  isCreator,
  deleteHandler,
  id,
}) => {
  const { showMessage } = useMessage();
  const { isAdmin, token, hasProfile } = useAuth();

  const formattedDate = book.publishedDate
    ? formatIsoDate(book.publishedDate, 'Publication date unknown')
    : 'Publication date unknown';

  const ratingsCount = book.ratingsCount ?? 0;

  return (
    <div className="book-info-card shadow-lg p-4">
      <div className="row g-0">
        <div className="col-md-4 book-info-image-container">
          {book.imageUrl ? (
            <img
              src={book.imageUrl}
              alt="Book Cover"
              className="book-info-image"
              onError={(e) => {
                (e.target as HTMLImageElement).src =
                  'https://via.placeholder.com/240x360?text=No+Image';
              }}
            />
          ) : (
            <div
              className="book-info-image book-info-image--placeholder"
              role="img"
              aria-label="No image available"
            />
          )}
        </div>
        <div className="col-md-8">
          <div className="card-body">
            <h2 className="book-title fw-bold">{book.title}</h2>
            <h5 className="book-author mb-3 text-muted">
              by {book.authorName || 'Unknown author'}
            </h5>
            <div className="d-flex align-items-center mb-3">
              <RenderStars rating={book.averageRating ?? 0} />
              <span className="ms-2 text-muted">
                ({ratingsCount} {ratingsCount === 1 ? 'review' : 'reviews'})
              </span>
            </div>
            <div className="genres mb-3">
              <span className="fw-semibold text-muted me-1">Genres:</span>
              {book.genres.map((g) => (
                <Link to={`${routes.genres}/${g.id}`} key={g.id}>
                  <span className="badge bg-secondary me-1">{g.name}</span>
                </Link>
              ))}
            </div>
            <p className="book-description card-text">
              {showFullDescription ? (book.longDescription ?? '') : `${descriptionPreview}...`}
            </p>
            {book.longDescription && book.longDescription.length > descriptionPreview.length && (
              <button
                onClick={() => setShowFullDescription((prev) => !prev)}
                className="btn btn-link p-0 text-decoration-none text-primary show-more-button"
              >
                {showFullDescription ? 'Show Less' : 'Show More'}
              </button>
            )}
            <p className="book-published-date text-muted mt-3">Published: {formattedDate}</p>
            <div className="read-buttons-section mt-4">
              {isAdmin ? (
                <ApproveRejectButtons
                  id={book.id}
                  initialIsApproved={!!book.isApproved}
                  token={token}
                  showMessage={showMessage}
                />
              ) : hasProfile ? (
                <ReadingListButtons
                  bookId={book.id}
                  initialReadingStatus={toUiStatus(book.readingStatus ?? null)}
                  token={token}
                  showMessage={showMessage}
                />
              ) : (
                <Link to={routes.profile}>Create Profile</Link>
              )}
            </div>
            <div className="d-flex gap-2 mt-4">
              {isCreator && (
                <Link
                  to={`${routes.editBook}/${id}`}
                  className="btn btn-warning d-flex align-items-center gap-2"
                >
                  <FaEdit /> Edit
                </Link>
              )}
              {(isCreator || isAdmin) && (
                <button
                  className="btn btn-danger d-flex align-items-center gap-2"
                  onClick={deleteHandler}
                >
                  <FaTrash /> Delete
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default BookFullInfo;
