import { useContext, useState, memo } from "react";
import ReactDOM from "react-dom";
import { MDBIcon, MDBBtn } from "mdb-react-ui-kit";

import * as reviewApi from "../../../../api/reviewApi";
import renderStars from "../../../../common/functions/renderStars";
import { errors } from "../../../../common/constants/messages";
import { routes } from "../../../../common/constants/api";
import { UserContext } from "../../../../contexts/userContext";

import DeleteModal from "../../../common/delete-modal/DeleteModal";

export default function ReviewItem({ review, onVote }) {
  const { userId, token, hasProfile } = useContext(UserContext);
  const { id, content, rating, creatorId, createdBy, upvotes, downvotes } =
    review;

  const [upvoteClicked, setUpvoteClicked] = useState(false);
  const [downvoteClicked, setDownvoteClicked] = useState(false);

  const [upvoteCount, setUpvoteCount] = useState(upvotes);
  const [downvoteCount, setDownvoteCount] = useState(downvotes);
  const [showModal, setShowModal] = useState(false);

  const toggleModal = () => setShowModal((prev) => !prev);

  const upvoteHandler = async () => {
    const success = await reviewApi.upvoteAsync(id, token);
    if (success) {
      if (downvoteClicked) {
        setDownvoteCount((prev) => --prev);
        setDownvoteClicked(false);
      }

      setUpvoteCount((prev) => ++prev);
      setUpvoteClicked(true);
    }
  };

  const downvoteHandler = async () => {
    const success = await reviewApi.downvoteAsync(id, token);
    if (success) {
      if (upvoteClicked) {
        setUpvoteCount((prev) => --prev);
        setUpvoteClicked(false);
      }

      setDownvoteCount((prev) => ++prev);
      setDownvoteClicked(true);
    }
  };

  const deleteHandler = async () => {
    if (showModal) {
      const success = await reviewApi.deleteAsync(id, token);

      if (success) {
        toggleModal();
        onVote();
      } else {
        toggleModal();
        navigate(routes.badRequest, {
          state: { message: errors.review.delete },
        });
      }
    }
  };

  return (
    <div className="review-item card shadow-sm p-3 mb-4 review-card">
      <div className="review-header d-flex justify-content-between align-items-center">
        <div className="rating-container">{renderStars(rating)}</div>
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
              style={{
                color: upvoteClicked ? "blue" : "black",
                cursor: "pointer",
              }}
              icon="arrow-up"
              className="vote-icon"
              onClick={upvoteHandler}
            />
            <span>{upvoteCount}</span>

            <MDBIcon
              style={{
                color: downvoteClicked ? "red" : "black",
                cursor: "pointer",
              }}
              icon="arrow-down"
              className="vote-icon ms-2"
              onClick={downvoteHandler}
            />
            <span>{downvoteCount}</span>
          </div>
        )}
        {userId === creatorId && (
          <div className="review-actions">
            <MDBBtn color="danger" size="sm" onClick={toggleModal}>
              Delete
            </MDBBtn>
          </div>
        )}
      </div>
      {showModal &&
        ReactDOM.createPortal(
          <DeleteModal
            showModal={showModal}
            toggleModal={toggleModal}
            deleteHandler={deleteHandler}
          />,
          document.getElementById("modal-root")
        )}
    </div>
  );
}
