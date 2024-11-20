import React from 'react'
import {
  MDBTextArea,
  MDBBtn,
  MDBIcon
} from 'mdb-react-ui-kit'

import './ReviewForm.css'

export default function ReviewForm() {
const [rating, setRating] = React.useState(0)

const handleRating = (value) => {
    setRating(value)
}

    return (
        <div className="review-form-container">
            <form className="review-form">
                <h4 className="text-center mb-4">Leave a Review</h4>
                <div className="rating-container mb-3">
                {[1, 2, 3, 4, 5].map(value => (
                    <MDBIcon
                    key={value}
                    icon="star"
                    className={`rating-star ${value <= rating ? 'active' : ''}`}
                    onClick={() => handleRating(value)}
                    />
                ))}
                </div>
                <MDBTextArea
                    wrapperClass="mb-3"
                    id="message"
                    rows={6}
                    label="Write your review"
                />
                <MDBBtn type="submit" className="mb-4" block>
                    Submit Review
                </MDBBtn>
            </form>
        </div>
    )
}
