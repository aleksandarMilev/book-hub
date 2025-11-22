import './AuthorDetails.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardText,
  MDBCardTitle,
  MDBCol,
  MDBContainer,
  MDBIcon,
  MDBRow,
} from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { FaEdit, FaTrash } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import ApproveRejectButtons from '@/features/author/components/details/approve-reject-buttons/ApproveRejectButtons.js';
import { useDetailsPage } from '@/features/author/hooks/useDetailsPage.js';
import BookListItem from '@/features/book/components/list-item/BookListItem.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { routes } from '@/shared/lib/constants/api.js';
import { calculateAge, formatIsoDate } from '@/shared/lib/utils/utils.js';

const AuthorDetails: FC = () => {
  const {
    parsedId,
    token,
    isAdmin,
    userId,
    author,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
    navigate,
    showMessage,
  } = useDetailsPage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !author) {
    return <DefaultSpinner />;
  }

  const isCreator = author.creatorId === userId;

  return (
    <MDBContainer className="my-5">
      <MDBRow>
        <MDBCol md="8" className="my-col">
          <MDBCard>
            <MDBCardBody>
              <MDBRow>
                <MDBCol md="12">
                  <MDBCardTitle className="author-title">{author.name}</MDBCardTitle>
                  <MDBCardText className="text-muted">
                    {author.nationality?.name ?? 'Nationality unknown'}
                    {author.penName ? ` \u00B7 Pen name: ${author.penName}` : ''}
                  </MDBCardText>
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12" className="author-image-container">
                  {author.imageUrl ? (
                    <img src={author.imageUrl} alt={author.name} className="author-image" />
                  ) : (
                    <div className="author-image-placeholder">No Image Available</div>
                  )}
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12">
                  <MDBCardText className="author-biography">{author.biography}</MDBCardText>
                  <MDBCardText className="text-muted">
                    <strong>Born:</strong>{' '}
                    {author.bornAt ? formatIsoDate(author.bornAt, 'Unknown') : 'Unknown'}
                    {author.bornAt && !author.diedAt
                      ? ` (${calculateAge(author.bornAt)} years old)`
                      : ''}
                  </MDBCardText>
                  {author.diedAt && (
                    <MDBCardText className="text-muted">
                      <strong>Died:</strong> {formatIsoDate(author.diedAt, 'Unknown')}
                      {author.bornAt &&
                        ` (${calculateAge(author.bornAt, author.diedAt)} years old)`}
                    </MDBCardText>
                  )}
                </MDBCol>
              </MDBRow>
              <MDBRow className="mt-4">
                <MDBCol md="6">
                  <div className="author-meta d-flex align-items-center">
                    <MDBIcon fas icon="star" className="me-2" />
                    <span className="me-2">Rating:</span>
                    <RenderStars rating={author.averageRating ?? 0} />
                  </div>
                </MDBCol>
                <MDBCol md="6">
                  <div className="author-meta">
                    <MDBIcon fas icon="book" className="me-2" />
                    {author.booksCount ?? 0} Books Published
                  </div>
                </MDBCol>
              </MDBRow>
              {(isCreator || isAdmin) && (
                <div className="d-flex gap-2 mt-4">
                  <Link
                    to={`${routes.editAuthor}/${parsedId}`}
                    className="btn btn-warning d-flex align-items-center gap-2"
                  >
                    <FaEdit /> Edit
                  </Link>
                  <button
                    type="button"
                    className="btn btn-danger d-flex align-items-center gap-2"
                    onClick={toggleModal}
                  >
                    <FaTrash /> Delete
                  </button>
                </div>
              )}
              {isAdmin && !author.isApproved && (
                <div className="mt-4">
                  <ApproveRejectButtons
                    authorId={author.id}
                    authorName={author.name}
                    initialIsApproved={author.isApproved ?? false}
                    token={token}
                    onSuccess={(msg, success) => showMessage(msg, !!success)}
                  />
                </div>
              )}
              <MDBRow className="mt-4">
                <MDBCol md="12">
                  <MDBCardTitle className="author-section-title">Top Books</MDBCardTitle>
                  {author.topBooks && author.topBooks.length > 0 ? (
                    <>
                      {author.topBooks.map((b) => (
                        <BookListItem key={b.id} {...b} />
                      ))}
                      <div className="d-flex justify-content-center mt-3">
                        <button
                          type="button"
                          className="btn btn-primary"
                          onClick={() =>
                            navigate(routes.book, {
                              state: { authorId: author.id, authorName: author.name },
                            })
                          }
                        >
                          View all {author.name} books
                        </button>
                      </div>
                    </>
                  ) : (
                    <MDBCardText>No books available for this author.</MDBCardText>
                  )}
                </MDBCol>
              </MDBRow>
            </MDBCardBody>
            <DeleteModal
              showModal={showModal}
              toggleModal={toggleModal}
              deleteHandler={deleteHandler}
            />
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default AuthorDetails;
