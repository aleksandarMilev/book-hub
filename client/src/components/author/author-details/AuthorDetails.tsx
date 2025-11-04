import { type FC, useContext } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { FaEdit, FaTrash } from 'react-icons/fa';
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBIcon,
} from 'mdb-react-ui-kit';
import { format } from 'date-fns';

import * as hooks from '../../../hooks/useAuthor';
import { routes } from '../../../common/constants/api';

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import DeleteModal from '../../common/delete-modal/DeleteModal';

import './AuthorDetails.css';
import { UserContext } from '../../../contexts/user/userContext';
import ApproveRejectButtons from './approve-reject-buttons/ApproveRejectButtons';
import BookListItem from '../../book/book-list-item/BooksListItem';
import { RenderStars } from '../../common/render-stars/renderStars';

const AuthorDetails: FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { token, isAdmin, userId } = useContext(UserContext);

  const { author, isFetching } = hooks.useDetails(id!);
  const { showModal, toggleModal, deleteHandler } = hooks.useRemove(id!, token, author?.name);

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
                    {author.bornAt ? format(new Date(author.bornAt), 'MMM dd, yyyy') : 'Unknown'}
                    {author.bornAt && !author.diedAt
                      ? ` (${
                          new Date().getFullYear() - new Date(author.bornAt).getFullYear()
                        } years old)`
                      : ''}
                  </MDBCardText>
                  {author.diedAt && (
                    <MDBCardText className="text-muted">
                      <strong>Died:</strong> {format(new Date(author.diedAt), 'MMM dd, yyyy')}
                      {author.bornAt &&
                        ` (${
                          new Date(author.diedAt).getFullYear() -
                          new Date(author.bornAt).getFullYear()
                        } years old)`}
                    </MDBCardText>
                  )}
                </MDBCol>
              </MDBRow>
              <MDBRow className="mt-4">
                <MDBCol md="6">
                  <div className="author-meta d-flex align-items-center">
                    <MDBIcon fas icon="star" className="me-2" />
                    <span className="me-2">Rating:</span>
                    //Argument of type 'number' is not assignable to parameter of type
                    'Props'.ts(2345)
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
                    to={`${routes.editAuthor}/${id}`}
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
                    onSuccess={() => {}}
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
                              state: { authorId: id, authorName: author.name },
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
