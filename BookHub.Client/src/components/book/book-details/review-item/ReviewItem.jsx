import { useContext, useState } from 'react'
import { MDBIcon } from 'mdb-react-ui-kit'

import renderStars from '../../../../common/functions/renderStars'
import { UserContext } from '../../../../contexts/userContext'

import './ReviewItem.css'

export default function ReviewItem({ review }) {
    const { userId } = useContext(UserContext)

    const { content, rating, creatorId, createdBy } = review

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
                        // className={`vote-icon ${userVote === 'upvote' ? 'active' : ''}`}
                        onClick={() => {}}
                    />
                    <MDBIcon
                        icon="arrow-down"
                        //className={`vote-icon ${userVote === 'downvote' ? 'active' : ''}`}
                        onClick={() => {}}
                    />
                </div>
                {userId === creatorId && (
                    <div className="review-actions">
                        <button className="btn btn-outline-primary btn-sm me-2" onClick={() => {}}>
                            Edit
                        </button>
                        <button className="btn btn-outline-danger btn-sm" onClick={() => {}}>
                            Delete
                        </button>
                    </div>
                )}
            </div>
        </div>
    )
}
