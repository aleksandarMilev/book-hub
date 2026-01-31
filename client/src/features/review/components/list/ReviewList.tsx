import './ReviewList.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';

import ReviewListItem from '@/features/review/components/list-item/ReviewListItem';
import { useAll } from '@/features/review/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';

const ReviewList: FC = () => {
  const { t } = useTranslation('reviews');

  const { reviews, isFetching, bookTitle, page, totalPages, handleNextPage, handlePreviousPage } =
    useAll();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (!reviews?.length) {
    return <p>{t('list.empty')}</p>;
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
      <h1>{bookTitle || t('list.titleFallback')}</h1>

      {reviews.map((r) => (
        <ReviewListItem key={r.id} review={r} />
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


