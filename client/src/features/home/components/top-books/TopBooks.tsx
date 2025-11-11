import './TopBooks.css';

import {
  MDBBtn,
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

import { useTopThree } from '@/hooks/useBook';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { routes } from '@/shared/lib/constants/api';

const TopBooks: FC = () => {
  const { books, isFetching, error } = useTopThree();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (error) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <div className="text-center">
          <FaBookReader size={100} color="red" className="mb-3" />
          <p className="lead">{error}</p>
        </div>
      </div>
    );
  }

  if (!books?.length) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <FaBookReader size={80} className="mb-3 text-muted" />
        <p className="lead text-muted">No top books available.</p>
      </div>
    );
  }

  return (
    <div className="top-books-container">
      <h2 className="top-books-title mb-4">Top Books</h2>
      <MDBCardGroup className="card-group">
        {books.map((b) => (
          <MDBCard key={b.id} className="top-book-card">
            <MDBCardImage
              src={b.imageUrl ?? undefined}
              alt={`${b.title} cover`}
              position="top"
              className="book-image"
            />
            <MDBCardBody>
              <MDBCardTitle className="card-title">
                <FaBook className="book-icon me-2" />
                {b.title}
              </MDBCardTitle>
              <MDBCardText>
                <strong>By:</strong> {b.authorName}
              </MDBCardText>
              <MDBCardText>{b.shortDescription}</MDBCardText>
              <MDBCardText>
                <RenderStars rating={b.averageRating ?? 0} />
              </MDBCardText>
              <MDBCardText>
                <strong>Genres:</strong>{' '}
                {b.genres && b.genres.length > 0 ? (
                  b.genres.map((g, i) => (
                    <Link key={g.id} to={`${routes.genres}/${g.id}`} className="genre-link">
                      <span className="genre-item">
                        {g.name}
                        {i < b.genres.length - 1 ? ', ' : ''}
                      </span>
                    </Link>
                  ))
                ) : (
                  <span>No genres available</span>
                )}
              </MDBCardText>
              <MDBBtn
                tag={Link}
                to={`${routes.book}/${b.id}`}
                color="dark"
                rounded
                size="sm"
                className="view-button"
              >
                View
              </MDBBtn>
            </MDBCardBody>
          </MDBCard>
        ))}
      </MDBCardGroup>
    </div>
  );
};

export default TopBooks;
