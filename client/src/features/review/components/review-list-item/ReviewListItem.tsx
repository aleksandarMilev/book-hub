import './ReviewListItem.css';

import { MDBBtn, MDBIcon } from 'mdb-react-ui-kit';
import { type FC, useState } from 'react';

import { useRemove } from '@/features/review/hooks/useCrud';
import { useVoteHandlers } from '@/features/review/hooks/useVote';
import type { Review } from '@/features/review/types/review';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { useAuth } from '@/shared/stores/auth/auth';

const ReviewListItem: FC<{
  review: Review;
  onVote: () => void | Promise<void>;
}> = ({ review, onVote }) => {
  const { userId, hasProfile } = useAuth();
  const { id, content, rating, creatorId, createdBy, upvotes, downvotes } = review;

  const [upvoteClicked, setUpvoteClicked] = useState(false);
  const [downvoteClicked, setDownvoteClicked] = useState(false);
  const [upvoteCount, setUpvoteCount] = useState<number>(upvotes ?? 0);
  const [downvoteCount, setDownvoteCount] = useState<number>(downvotes ?? 0);

  const { showModal, toggleModal, deleteHandler } = useRemove(id, onVote);
  const { handleUpvote, handleDownvote } = useVoteHandlers({
    id,
    hasProfile,
    upvoteCount,
    downvoteCount,
    setUpvoteCount,
    setDownvoteCount,
    setUpvoteClicked,
    setDownvoteClicked,
    onVote,
  });

  return (
    <div className="review-item card shadow-sm p-3 mb-4 review-card">
      <div className="review-header d-flex justify-content-between align-items-center">
        <div className="rating-container">
          <RenderStars rating={rating ?? 0} />
        </div>
        <div className="creator-info d-flex align-items-center">
          <span className="creator-id">{createdBy}</span>
          <MDBIcon icon="user-circle" className="user-icon ms-2" />
        </div>
      </div>
      <p className="review-content">{content}</p>
      <div className="review-footer d-flex justify-content-between align-items-center">
        {hasProfile && (
          <div className="review-votes d-flex align-items-center">
            <MDBIcon
              style={{ color: upvoteClicked ? 'blue' : 'black', cursor: 'pointer' }}
              icon="arrow-up"
              className="vote-icon"
              onClick={handleUpvote}
            />
            <span>{upvoteCount}</span>
            <MDBIcon
              style={{ color: downvoteClicked ? 'red' : 'black', cursor: 'pointer' }}
              icon="arrow-down"
              className="vote-icon ms-2"
              onClick={handleDownvote}
            />
            <span>{downvoteCount}</span>
          </div>
        )}
        {String(userId) === String(creatorId) && (
          <div className="review-actions">
            <MDBBtn color="danger" size="sm" onClick={toggleModal}>
              Delete
            </MDBBtn>
          </div>
        )}
      </div>
      <DeleteModal showModal={showModal} toggleModal={toggleModal} deleteHandler={deleteHandler} />
    </div>
  );
};

export default ReviewListItem;
