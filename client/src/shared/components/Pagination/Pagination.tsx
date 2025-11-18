import './Pagination.css';

import type { FC } from 'react';
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa';

const Pagination: FC<{
  page: number;
  totalPages: number;
  disabled?: boolean;
  onPageChange: (newPage: number) => void;
}> = ({ page, totalPages, disabled = false, onPageChange }) => {
  if (totalPages <= 1) return null;

  const prev = () => page > 1 && onPageChange(page - 1);
  const next = () => page < totalPages && onPageChange(page + 1);

  return (
    <div className="bh-pagination">
      <button
        className="bh-page-btn"
        onClick={prev}
        disabled={page === 1 || disabled}
        aria-label="Previous page"
      >
        <FaArrowLeft /> <span>Previous</span>
      </button>
      <div className="bh-page-counter">
        <span className="current">{page}</span>
        <span className="divider">/</span>
        <span className="total">{totalPages}</span>
      </div>
      <button
        className="bh-page-btn"
        onClick={next}
        disabled={page === totalPages || disabled}
        aria-label="Next page"
      >
        <span>Next</span> <FaArrowRight />
      </button>
    </div>
  );
};

export default Pagination;
