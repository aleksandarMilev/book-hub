import './ReviewListItem.css';

import { MDBIcon } from 'mdb-react-ui-kit';
import { type FC, useState } from 'react';
import { createPortal } from 'react-dom';
import { useTranslation } from 'react-i18next';

import { useRemove } from '@/features/review/hooks/useCrud.js';
import { useVoteHandlers } from '@/features/review/hooks/useVote.js';
import type { Review } from '@/features/review/types/review.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

type Props = {
  review: Review;
  onVote?: () => void | Promise<void>;
};

const ReviewListItem: FC<Props> = ({ review, onVote }) => {
  const { t } = useTranslation('reviews');

  const { userId, isAuthenticated } = useAuth();
  const { id, content, rating, creatorId, createdBy, upvotes, downvotes } = review;

  const [upvoteClicked, setUpvoteClicked] = useState(false);
  const [downvoteClicked, setDownvoteClicked] = useState(false);
  const [upvoteCount, setUpvoteCount] = useState<number>(upvotes ?? 0);
  const [downvoteCount, setDownvoteCount] = useState<number>(downvotes ?? 0);

  const { showModal, toggleModal, deleteHandler } = useRemove(
    id,
    onVote ? () => onVote() : undefined,
  );

  const { handleUpvote, handleDownvote } = useVoteHandlers({
    id,
    isAuthenticated,
    upvoteCount,
    downvoteCount,
    setUpvoteCount,
    setDownvoteCount,
    setUpvoteClicked,
    setDownvoteClicked,
  });

  return (
    <article className="review-item review-card" aria-label="Review">
      <header className="review-header">
        <div className="rating-container">
          <RenderStars rating={rating ?? 0} />
        </div>

        <div className="creator-info">
          <span className="creator-id">{createdBy}</span>
          <MDBIcon icon="user-circle" className="user-icon" />
        </div>
      </header>

      <p className="review-content">{content}</p>

      <footer className="review-footer">
        {isAuthenticated && (
          <div className="review-votes" aria-label="Votes">
            <button
              type="button"
              className={`vote-btn ${upvoteClicked ? 'vote-btn--up-active' : ''}`}
              onClick={handleUpvote}
              aria-label="Upvote"
            >
              <MDBIcon icon="arrow-up" className="vote-icon" />
            </button>
            <span className="vote-count" aria-label="Upvotes">
              {upvoteCount}
            </span>

            <button
              type="button"
              className={`vote-btn ${downvoteClicked ? 'vote-btn--down-active' : ''}`}
              onClick={handleDownvote}
              aria-label="Downvote"
            >
              <MDBIcon icon="arrow-down" className="vote-icon" />
            </button>
            <span className="vote-count" aria-label="Downvotes">
              {downvoteCount}
            </span>
          </div>
        )}

        {String(userId) === String(creatorId) && (
          <div className="review-actions">
            <button type="button" className="review-delete-btn" onClick={toggleModal}>
              {t('item.delete')}
            </button>
          </div>
        )}
      </footer>

      {typeof document !== 'undefined' &&
        createPortal(
          <DeleteModal
            showModal={showModal}
            toggleModal={toggleModal}
            deleteHandler={deleteHandler}
          />,
          document.body,
        )}
    </article>
  );
};

export default ReviewListItem;
