import './BookDetails.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { Link } from 'react-router-dom';

import AuthorIntroduction from '@/features/author/components/author-introduction/AuthorIntroduction';
import BookFullInfo from '@/features/book/components/details/full-info/BookFullInfo';
import { useDetailsPage } from '@/features/book/hooks/useDetailsPage';
import CreateReview from '@/features/review/components/create-review/CreateReview';
import EditReview from '@/features/review/components/edit-review/EditReview';
import ReviewListItem from '@/features/review/components/review-list-item/ReviewListItem';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';
import { routes } from '@/shared/lib/constants/api';

const BookDetails: FC = () => {
  const {
    showFullDescription,
    userId,
    hasProfile,
    isAdmin,
    error,
    isFetching,
    book,
    setShowFullDescription,
    toggleModal,
    parsedId,
    setIsReviewEdited,
    refreshBook,
    firstReviewRef,
    showModal,
    deleteHandler,
    setIsReviewCreated,
  } = useDetailsPage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !book) {
    return <DefaultSpinner />;
  }

  const isCreator = String(userId) === String(book.creatorId);
  const previewTextLength = 200;
  const descriptionPreview = (book.longDescription ?? '').slice(0, previewTextLength);
  const existingReview = book.reviews?.find((r) => String(r.creatorId) === String(userId));

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
                id={parsedId!}
              />
              <AuthorIntroduction author={book.author!} />
              {!isAdmin && hasProfile ? (
                <>
                  {!existingReview && (
                    <CreateReview
                      bookId={parsedId!}
                      refreshReviews={refreshBook}
                      setIsReviewCreated={setIsReviewCreated}
                    />
                  )}
                  {existingReview && (
                    <EditReview
                      bookId={parsedId!}
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
                  book.reviews.map((r, i) => (
                    <div ref={i === 0 ? firstReviewRef : null} key={r.id}>
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
