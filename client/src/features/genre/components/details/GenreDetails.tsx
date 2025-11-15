import './GenreDetails.css';

import { MDBBtn, MDBCardText, MDBTypography } from 'mdb-react-ui-kit';
import { type FC } from 'react';

import BookListItem from '@/features/book/components/list-item/BookListItem';
import { useGenreDetailsPage } from '@/features/genre/hooks/useGenreDetailsPage';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';

const GenreDetails: FC = () => {
  const { genre, isFetching, error, handleAllBooksClick } = useGenreDetailsPage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !genre) {
    return <DefaultSpinner />;
  }

  return (
    <div className="container genre-details mt-5">
      <div className="card shadow-lg p-3 mb-5 bg-white rounded">
        <div className="row g-0">
          {genre.imageUrl && (
            <div className="col-md-4 genre-image">
              <img
                src={genre.imageUrl}
                alt={`${genre.name} genre`}
                className="img-fluid rounded-start"
              />
            </div>
          )}
          <div className={'col-md-8'}>
            <div className="card-body">
              <h1 className="card-title text-primary">{genre.name}</h1>
              <p className="card-text text-muted">{genre.description}</p>
            </div>
          </div>
        </div>
      </div>
      <div className="top-books-section mt-5">
        <MDBTypography tag="h4" className="section-title">
          Top Books
        </MDBTypography>
        {genre.topBooks && genre.topBooks.length > 0 ? (
          <>
            {genre.topBooks.map((b) => (
              <BookListItem key={b.id} {...b} />
            ))}
            <div className="d-flex justify-content-center mt-3">
              <MDBBtn onClick={handleAllBooksClick}>View all {genre.name} books</MDBBtn>
            </div>
          </>
        ) : (
          <MDBCardText>No books available for this genre.</MDBCardText>
        )}
      </div>
    </div>
  );
};

export default GenreDetails;
