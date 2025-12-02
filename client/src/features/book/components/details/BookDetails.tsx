import './BookDetails.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import AuthorIntroduction from '@/features/author/components/introduction/AuthorIntroduction.js';
import BookFullInfo from '@/features/book/components/details/full-info/BookFullInfo.js';
import { useDetailsPage } from '@/features/book/hooks/useDetailsPage.js';
import CreateReview from '@/features/review/components/create-review/CreateReview.js';
import EditReview from '@/features/review/components/edit-review/EditReview.js';
import ReviewListItem from '@/features/review/components/review-list-item/ReviewListItem.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.js';
import { routes } from '@/shared/lib/constants/api.js';

const BookDetails: FC = () => {
  const { t } = useTranslation('books');

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
    id,
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
    <MDBContainer className="book-details-page my-5">
      <MDBRow>
        <MDBCol md="8" className="book-details-col">
          <MDBCard className="book-details-card">
            <MDBCardBody className="book-details-body">
              <BookFullInfo
                book={book}
                descriptionPreview={descriptionPreview}
                showFullDescription={showFullDescription}
                setShowFullDescription={setShowFullDescription}
                isCreator={isCreator}
                deleteHandler={toggleModal}
              />
              {book.author && <AuthorIntroduction author={book.author} />}
              {!isAdmin && hasProfile ? (
                <>
                  {!existingReview && (
                    <CreateReview
                      bookId={id}
                      refreshReviews={refreshBook}
                      setIsReviewCreated={setIsReviewCreated}
                    />
                  )}
                  {existingReview && (
                    <EditReview
                      bookId={id}
                      existingReview={existingReview}
                      setIsReviewEdited={setIsReviewEdited}
                      refreshReviews={refreshBook}
                    />
                  )}
                </>
              ) : (
                !isAdmin && (
                  <>
                    <h3 className="create-review-heading">{t('details.createReviewTitle')}</h3>
                    <div className="create-profile-container">
                      <Link className="create-profile-link" to={routes.profile}>
                        {t('details.createProfile')}
                      </Link>
                    </div>
                  </>
                )
              )}
              <div className="reviews-section mt-4 text-center">
                <h5 className="reviews-title">{t('details.reviewsTitle')}</h5>
                {book.reviews && book.reviews.length > 0 ? (
                  book.reviews.map((r, i) => (
                    <div ref={i === 0 ? firstReviewRef : null} key={r.id}>
                      <ReviewListItem review={r} onVote={refreshBook} />
                    </div>
                  ))
                ) : (
                  <p className="no-reviews-message">{t('details.noReviews')}</p>
                )}
                {book.moreThanFiveReviews && (
                  <Link
                    className="reviews-button"
                    to={`${routes.review}/${book.id}`}
                    state={book.title}
                  >
                    {t('details.allReviews')}
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
