import { type FC } from 'react';
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa';
import './Pagination.css';
import type { PaginationProps } from './types/paginationProps';

const Pagination: FC<PaginationProps> = ({ page, totalPages, disabled = false, onPageChange }) => {
  const handlePrevious = () => {
    if (page > 1) {
      onPageChange(page - 1);
    }
  };

  const handleNext = () => {
    if (page < totalPages) {
      onPageChange(page + 1);
    }
  };

  if (totalPages <= 1) {
    return null;
  }

  return (
    <div className="pagination-container d-flex justify-content-center mt-4 align-items-center">
      <button
        className="btn pagination-btn me-3"
        onClick={handlePrevious}
        disabled={page === 1 || disabled}
        aria-label="Previous page"
      >
        <FaArrowLeft /> Previous
      </button>

      <div className="pagination-info">
        <span className="current-page fw-bold">{page}</span> /{' '}
        <span className="total-pages">{totalPages}</span>
      </div>

      <button
        className="btn pagination-btn ms-3"
        onClick={handleNext}
        disabled={page === totalPages || disabled}
        aria-label="Next page"
      >
        Next <FaArrowRight />
      </button>
    </div>
  );
};

export default Pagination;
