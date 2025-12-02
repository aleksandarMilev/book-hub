import './BookFullInfo.css';

import type React from 'react';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaEdit, FaTrash } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { BookDetails } from '@/features/book/types/book.js';
import { toUiStatus } from '@/features/reading-list/types/readingList.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate, getImageUrl } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

import { ApproveRejectButtons } from './approve-reject-buttons/ApproveRejectButtons.js';
import { ReadingListButtons } from './reading-list-buttons/ReadingListButtons.js';

type Props = {
  book: BookDetails;
  descriptionPreview: string;
  showFullDescription: boolean;
  setShowFullDescription: React.Dispatch<React.SetStateAction<boolean>>;
  isCreator: boolean;
  deleteHandler: () => void;
};

const BookFullInfo: FC<Props> = ({
  book,
  descriptionPreview,
  showFullDescription,
  setShowFullDescription,
  isCreator,
  deleteHandler,
}) => {
  const { t } = useTranslation('books');
  const { showMessage } = useMessage();
  const { isAdmin, token, hasProfile } = useAuth();

  const fallbackDateText = 'Publication date unknown';
  const formattedDate = book.publishedDate
    ? formatIsoDate(book.publishedDate, fallbackDateText)
    : fallbackDateText;

  const ratingsCount = book.ratingsCount ?? 0;
  const authorDisplayName = book.authorName || t('list.unknownAuthor');

  return (
    <div className="book-info-card shadow-lg">
      <div className="row g-0">
        <nav aria-label="breadcrumb" className="book-details-breadcrumb">
          <ol className="breadcrumb mb-2">
            <li className="breadcrumb-item">
              <Link to={routes.home}>{t('details.breadcrumb.home')}</Link>
            </li>
            <li className="breadcrumb-item">
              <Link to={routes.book}>{t('details.breadcrumb.list')}</Link>
            </li>
            <li className="breadcrumb-item active" aria-current="page">
              {book.title}
            </li>
          </ol>
        </nav>
        <div className="col-md-4 book-info-image-container">
          <img
            src={getImageUrl(book.imagePath, 'books')}
            alt={book.title}
            className="book-info-image"
          />
        </div>
        <div className="col-md-8">
          <div className="card-body book-info-body">
            <h2 className="book-title fw-bold">{book.title}</h2>
            <h5 className="book-author mb-3 text-muted">
              {t('list.byAuthor', { author: authorDisplayName })}
            </h5>
            <div className="d-flex align-items-center mb-3 book-info-rating">
              <RenderStars rating={book.averageRating ?? 0} />
              <span className="ms-2 text-muted book-info-rating-count">
                ({ratingsCount} {t('details.reviewsTitle')})
              </span>
            </div>
            <div className="genres mb-3 book-info-genres">
              <span className="fw-semibold text-muted me-1">{t('details.genresLabel')}</span>
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
                {showFullDescription ? t('details.showLess') : t('details.showMore')}
              </button>
            )}
            <p className="book-published-date text-muted mt-3">
              {t('details.publishedLabel')} {formattedDate}
            </p>
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
                <Link to={routes.profile} className="book-create-profile-link">
                  {t('details.createProfile')}
                </Link>
              )}
            </div>
            <div className="book-info-actions mt-4">
              {isCreator && (
                <Link
                  to={`${routes.editBook}/${book.id}`}
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
