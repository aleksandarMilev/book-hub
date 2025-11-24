import './Pagination.css';

import { type FC, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa';

const Pagination: FC<{
  page: number;
  totalPages: number;
  disabled?: boolean;
  onPageChange: (newPage: number) => void;
}> = ({ page, totalPages, disabled = false, onPageChange }) => {
  const { t } = useTranslation('common');

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, [page]);

  if (totalPages <= 1) {
    return null;
  }

  const prev = () => {
    if (page > 1) {
      onPageChange(page - 1);
    }
  };

  const next = () => {
    if (page < totalPages) {
      onPageChange(page + 1);
    }
  };

  return (
    <div className="bh-pagination">
      <button
        className="bh-page-btn"
        onClick={prev}
        disabled={page === 1 || disabled}
        aria-label={t('pagination.ariaPrevious')}
      >
        <FaArrowLeft /> <span>{t('pagination.previous')}</span>
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
        aria-label={t('pagination.ariaNext')}
      >
        <span>{t('pagination.next')}</span> <FaArrowRight />
      </button>
    </div>
  );
};

export default Pagination;
