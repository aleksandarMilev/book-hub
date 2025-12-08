import './GenreDetails.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardText,
  MDBCardTitle,
  MDBCol,
  MDBContainer,
  MDBRow,
  MDBTypography,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import BookListItem from '@/features/book/components/list-item/BookListItem.js';
import { useGenreDetailsPage } from '@/features/genre/hooks/useGenreDetailsPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.js';
import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl } from '@/shared/lib/utils/utils.js';

const GenreDetails: FC = () => {
  const { t } = useTranslation('genres');
  const { genre, isFetching, error, handleAllBooksClick } = useGenreDetailsPage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !genre) {
    return <DefaultSpinner />;
  }

  return (
    <MDBContainer className="genre-details-page my-5">
      <MDBRow>
        <MDBCol md="8" className="genre-details-col">
          <MDBCard className="genre-details-card">
            <MDBCardBody className="genre-details-body">
              <nav aria-label="breadcrumb" className="genre-details-breadcrumb">
                <ol className="breadcrumb mb-2">
                  <li className="breadcrumb-item">
                    <Link to={routes.home}>{t('details.breadcrumb.home')}</Link>
                  </li>
                  <li className="breadcrumb-item">
                    <Link to={routes.genres}>{t('details.breadcrumb.list')}</Link>
                  </li>
                  <li className="breadcrumb-item active" aria-current="page">
                    {genre.name}
                  </li>
                </ol>
              </nav>
              <MDBRow className="genre-header-row align-items-center">
                <MDBCol
                  md="4"
                  className="d-flex justify-content-center justify-content-md-start mb-3 mb-md-0"
                >
                  <div className="genre-image-container">
                    {genre.imagePath ? (
                      <img
                        src={getImageUrl(genre.imagePath, 'genres')}
                        alt={genre.name}
                        className="genre-details-image"
                      />
                    ) : (
                      <div className="genre-details-image-placeholder">
                        {t('details.image.noImage')}
                      </div>
                    )}
                  </div>
                </MDBCol>
                <MDBCol md="8">
                  <MDBCardTitle className="genre-title">{genre.name}</MDBCardTitle>
                  <MDBCardText className="genre-description text-muted">
                    {genre.description}
                  </MDBCardText>
                </MDBCol>
              </MDBRow>
              <MDBRow className="genre-top-books mt-4">
                <MDBCol md="12">
                  <MDBTypography tag="h4" className="genre-section-title">
                    {t('details.topBooks.title')}
                  </MDBTypography>
                  {genre.topBooks && genre.topBooks.length > 0 ? (
                    <>
                      {genre.topBooks.map((b) => (
                        <BookListItem key={b.id} {...b} />
                      ))}
                      <div className="d-flex justify-content-center mt-3">
                        <button
                          type="button"
                          className="genre-view-all-btn"
                          onClick={handleAllBooksClick}
                        >
                          {t('details.topBooks.viewAll', { name: genre.name })}
                        </button>
                      </div>
                    </>
                  ) : (
                    <MDBCardText className="text-muted">{t('details.topBooks.none')}</MDBCardText>
                  )}
                </MDBCol>
              </MDBRow>
            </MDBCardBody>
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default GenreDetails;
