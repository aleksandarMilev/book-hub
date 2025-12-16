import './TopBooks.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardGroup,
  MDBCardImage,
  MDBCardText,
  MDBCardTitle,
} from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { FaBook, FaBookReader } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { useTopThree } from '@/features/book/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import EmptyState from '@/shared/components/empty-state/EmptyState.js';
import HomePageError from '@/shared/components/errors/home-page/HomePageError.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl, slugify } from '@/shared/lib/utils/utils.js';

const TopBooks: FC = () => {
  const { t, books, isFetching, error } = useTopThree();

  if (error) {
    return <HomePageError message={error} />;
  }

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (!books?.length) {
    return (
      <EmptyState
        icon={<FaBookReader />}
        title={t('topBooks.emptyTitle')}
        message={t('topBooks.emptyMessage')}
      />
    );
  }

  return (
    <div className="top-books-container">
      <h2 className="top-books-title mb-4">{t('topBooks.title')}</h2>
      <MDBCardGroup className="card-group">
        {books.map((b) => (
          <MDBCard key={b.id} className="top-book-card">
            <MDBCardImage
              src={getImageUrl(b.imagePath, 'books')}
              alt={t('topBooks.labels.bookCoverAlt', { title: b.title })}
              position="top"
              className="book-image"
            />
            <MDBCardBody>
              <MDBCardTitle className="card-title">
                <FaBook className="book-icon me-2" />
                {b.title}
              </MDBCardTitle>
              <MDBCardText>
                <strong>{t('topBooks.labels.by')}</strong> {b.authorName}
              </MDBCardText>
              <MDBCardText>{b.shortDescription}</MDBCardText>
              <MDBCardText className="rating-text">
                <RenderStars rating={b.averageRating ?? 0} />
              </MDBCardText>
              <MDBCardText className="genres-wrapper">
                <strong>{t('topBooks.labels.genres')}</strong>
                <div className="genre-list">
                  {b.genres && b.genres.length > 0 ? (
                    b.genres.map((g) => (
                      <Link
                        key={g.id}
                        to={`${routes.genres}/${g.id}/${slugify(g.name)}`}
                        className="genre-link"
                      >
                        <span className="genre-item">{g.name}</span>
                      </Link>
                    ))
                  ) : (
                    <span>{t('topBooks.labels.noGenres')}</span>
                  )}
                </div>
              </MDBCardText>
              <Link to={`${routes.book}/${b.id}`} className="book-view-button">
                {t('topBooks.labels.view')}
              </Link>
            </MDBCardBody>
          </MDBCard>
        ))}
      </MDBCardGroup>
    </div>
  );
};

export default TopBooks;
