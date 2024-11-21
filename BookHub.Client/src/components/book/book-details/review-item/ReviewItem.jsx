import React, { useContext, useState } from 'react'
import ReactDOM from 'react-dom'
import { useNavigate } from 'react-router-dom'
import { MDBIcon, MDBBtn } from 'mdb-react-ui-kit'

import * as reviewApi from '../../../../api/reviewApi'
import * as useReview from '../../../../hooks/useReview'
import renderStars from '../../../../common/functions/renderStars'
import { errors } from '../../../../common/constants/messages'
import { routes } from '../../../../common/constants/api'
import { UserContext } from '../../../../contexts/userContext'

import DeleteModal from '../../../common/delete-modal/DeleteModal'

import './ReviewItem.css'

export default function ReviewItem({ review, refreshReviews }) {
    const navigate = useNavigate()

    const upvoteHandler = useReview.useUpvote()
    const downvoteHandler = useReview.useDownvote()

    const { userId, token } = useContext(UserContext)
    const { id, content, rating, creatorId, createdBy, upvotes, downvotes } = review

    const [upvoteCount, setUpvoteCount] = useState(upvotes)
    const [downvoteCount, setDownvoteCount] = useState(downvotes)

    const [showModal, setShowModal] = useState(false)
    const toggleModal = () => setShowModal(old => !old)

    async function deleteHandler() {
        if (showModal) {
            const success = await reviewApi.deleteAsync(id, token)

            if (success) {
                refreshReviews()
                toggleModal()
            } else {
                toggleModal()
                navigate(routes.badRequest, { state: { message: errors.review.delete } })
            }
        }
    }

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
                <div className="review-votes d-flex align-items-center">
                    <MDBIcon
                        icon="arrow-up"
                        className="vote-icon"
                        onClick={() => upvoteHandler(id, setUpvoteCount)}
                    />
                    <span>{upvoteCount}</span>

                    <MDBIcon
                        icon="arrow-down"
                        className="vote-icon ms-2"
                        onClick={() => downvoteHandler(id, setDownvoteCount)}
                    />
                    <span>{downvoteCount}</span>
                </div>
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
                    document.getElementById('modal-root') 
                )}
        </div>
    )
}
