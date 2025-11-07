import { type FC } from 'react';

import { useReviewList } from '../../../hooks/useReview';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import Pagination from '../../common/pagination/Pagination'; // ✅ import shared component

import ReviewListItem from '../review-list-item/ReviewListItem';

import './ReviewList.css';

const ReviewList: FC = () => {
  const {
    reviews,
    isFetching,
    bookTitle,
    page,
    totalPages,
    handleNextPage,
    handlePreviousPage,
    fetchData,
  } = useReviewList();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (!reviews?.length) {
    return <p>No reviews found.</p>;
  }

  const handlePageChange = (newPage: number) => {
    if (newPage > page) {
      handleNextPage();
    } else if (newPage < page) {
      handlePreviousPage();
    }
  };

  return (
    <div className="review-list">
      <h1>{bookTitle || 'Reviews'}</h1>

      {reviews.map((r) => (
        <ReviewListItem key={r.id} review={r} onVote={() => fetchData(page)} />
      ))}

      <Pagination
        page={page}
        totalPages={totalPages}
        disabled={isFetching}
        onPageChange={handlePageChange}
      />
    </div>
  );
};

export default ReviewList;
