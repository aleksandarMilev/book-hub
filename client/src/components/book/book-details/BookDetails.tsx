import { type FC, useContext, useEffect, useRef, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { MDBContainer, MDBRow, MDBCol, MDBCard, MDBCardBody } from 'mdb-react-ui-kit';

import * as hooks from '../../../hooks/useBook';
import { routes } from '../../../common/constants/api';
import { UserContext } from '../../../contexts/user/userContext';
import { parseId } from '../../../common/functions/utils';

import BookFullInfo from './book-full-info/BookFullInfo';
import AuthorIntroduction from '../../author/author-introduction/AuthorIntroduction';
import ReviewListItem from '../../review/review-list-item/ReviewListItem';
import EditReview from '../../review/edit-review/EditReview';
import CreateReview from '../../review/create-review/CreateReview';
import DeleteModal from '../../common/delete-modal/DeleteModal';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';

import './BookDetails.css';

const BookDetails: FC = () => {
  const { id } = useParams<{ id: string }>();
  let parsedId: number | null = null;

  try {
    parsedId = parseId(id);
  } catch {}

  if (parsedId == null) {
    return <div>Invalid book id.</div>;
  }

  const firstReviewRef = useRef<HTMLDivElement | null>(null);
  const [showFullDescription, setShowFullDescription] = useState(false);

  const { userId, hasProfile, isAdmin } = useContext(UserContext);

  const { book, isFetching, refreshBook } = hooks.useFullInfo(parsedId);
  const { showModal, toggleModal, deleteHandler } = hooks.useRemove(parsedId, book?.title);

  const [isReviewCreated, setIsReviewCreated] = useState(false);
  const [isReviewEdited, setIsReviewEdited] = useState(false);

  useEffect(() => {
    if ((isReviewCreated || isReviewEdited) && firstReviewRef.current) {
      firstReviewRef.current.scrollIntoView({ behavior: 'smooth' });

      setIsReviewCreated(false);
      setIsReviewEdited(false);
    }
  }, [isReviewCreated, isReviewEdited, book?.reviews]);

  if (isFetching || !book) {
    return <DefaultSpinner />;
  }

  const isCreator = String(userId) === String(book.creatorId);
  const previewTextLength = 200;
  const descriptionPreview = (book.longDescription ?? '').slice(0, previewTextLength);
  const existingReview = book.reviews?.find((r: any) => String(r.creatorId) === String(userId));

  return (
    <MDBContainer className="my-5">
      <MDBRow>
        <MDBCol md="8" className="my-col">
          <MDBCard>
            <MDBCardBody>
              <BookFullInfo
                book={book}
                descriptionPreview={descriptionPreview}
                showFullDescription={showFullDescription}
                setShowFullDescription={setShowFullDescription}
                isCreator={isCreator}
                deleteHandler={toggleModal}
                id={parsedId}
              />
              <AuthorIntroduction author={book.author} />
              {!isAdmin && hasProfile ? (
                <>
                  {!existingReview && (
                    <CreateReview
                      bookId={parsedId}
                      refreshReviews={refreshBook}
                      setIsReviewCreated={setIsReviewCreated}
                    />
                  )}
                  {existingReview && (
                    <EditReview
                      bookId={parsedId}
                      existingReview={existingReview}
                      setIsReviewEdited={setIsReviewEdited}
                      refreshReviews={refreshBook}
                    />
                  )}
                </>
              ) : (
                !isAdmin && (
                  <>
                    <h3 className="create-review-heading">Create Review</h3>
                    <div className="create-profile-container">
                      <Link className="create-profile-link" to={routes.profile}>
                        Create Profile
                      </Link>
                    </div>
                  </>
                )
              )}
              <div className="reviews-section mt-4 text-center">
                <h5 className="reviews-title">Reviews</h5>
                {book.reviews && book.reviews.length > 0 ? (
                  book.reviews.map((r: any, index: number) => (
                    <div ref={index === 0 ? firstReviewRef : null} key={r.id}>
                      <ReviewListItem review={r} onVote={refreshBook} />
                    </div>
                  ))
                ) : (
                  <p className="no-reviews-message">No reviews yet.</p>
                )}
                {book.moreThanFiveReviews && (
                  <Link
                    className="reviews-button"
                    to={`${routes.review}/${book.id}`}
                    state={book.title}
                  >
                    All Reviews
                  </Link>
                )}
              </div>
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

export default BookDetails;
