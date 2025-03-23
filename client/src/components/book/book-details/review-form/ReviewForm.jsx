import { useState } from "react";
import { useFormik } from "formik";
import * as Yup from "yup";
import { MDBTextArea, MDBBtn, MDBIcon } from "mdb-react-ui-kit";

import * as useReview from "../../../../hooks/useReview";

import "./ReviewForm.css";

export default function ReviewForm({
  bookId,
  refreshReviews,
  setIsReviewCreatedOrEdited,
  existingReview = null,
}) {
  const [rating, setRating] = useState(existingReview?.rating || 0);

  const createReviewHandler = useReview.useCreate();
  const editReviewHandler = useReview.useEdit();

  const isEditMode = !!existingReview;

  const formik = useFormik({
    initialValues: {
      content: existingReview?.content || "",
      rating,
    },
    validationSchema: Yup.object({
      content: Yup.string()
        .min(10, "Review must be at least 10 characters")
        .max(5000, "Review cannot exceed 5000 characters")
        .required("Review content is required"),
      rating: Yup.number()
        .min(1, "Please select a rating")
        .max(5, "Rating cannot exceed 5")
        .required("Please select a rating"),
    }),
    onSubmit: async (values, { setErrors, resetForm }) => {
      try {
        const reviewData = { ...values, bookId };

        if (isEditMode) {
          const isSuccessfullyEdited = await editReviewHandler(
            existingReview.id,
            reviewData
          );

          if (isSuccessfullyEdited) {
            setIsReviewCreatedOrEdited(true);
            refreshReviews();
            resetForm({ values: { content: "", rating: 0 } });
            setRating(0);
          }
        } else {
          const reviewId = await createReviewHandler(reviewData);

          if (reviewId) {
            setIsReviewCreatedOrEdited(true);
            refreshReviews();
            resetForm({ values: { content: "", rating: 0 } });
            setRating(0);
          }
        }
      } catch (error) {
        setErrors({ submit: "Error submitting review. Please try again." });
      }
    },
  });

  const handleRating = (value) => {
    setRating(value);
    formik.setFieldValue("rating", value);
  };

  return (
    <div className="review-form-container">
      <form className="review-form" onSubmit={formik.handleSubmit}>
        <h4 className="text-center mb-4">
          {isEditMode ? "Edit Review" : "Leave a Review"}
        </h4>
        <div className="rating-container mb-3">
          {[1, 2, 3, 4, 5].map((value) => (
            <MDBIcon
              key={value}
              icon="star"
              className={`rating-star ${value <= rating ? "active" : ""}`}
              onClick={() => handleRating(value)}
            />
          ))}
        </div>
        {formik.touched.rating && formik.errors.rating ? (
          <div className="error-text">{formik.errors.rating}</div>
        ) : null}
        <MDBTextArea
          wrapperClass="mb-3"
          id="content"
          rows={6}
          label="Write your review"
          {...formik.getFieldProps("content")}
        />
        {formik.touched.content && formik.errors.content ? (
          <div className="error-text">{formik.errors.content}</div>
        ) : null}
        <MDBBtn
          type="submit"
          className="mb-4"
          block
          disabled={formik.isSubmitting}
        >
          {isEditMode ? "Update Review" : "Submit Review"}
        </MDBBtn>
      </form>
    </div>
  );
}
