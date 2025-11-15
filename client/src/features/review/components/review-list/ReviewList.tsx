import './ReviewList.css';

import { type FC } from 'react';

import ReviewListItem from '@/features/review/components/review-list-item/ReviewListItem.js';
import { useAll } from '@/features/review/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import Pagination from '@/shared/components/pagination/Pagination.js';

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
  } = useAll();

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
