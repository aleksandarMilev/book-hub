// features/reading-list/components/list/ReadingList.tsx
import './ReadingList.css';

import { type FC } from 'react';
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa';

import BookListItem from '@/features/book/components/list-item/BookListItem';
import { getTitle, getTotalPages } from '@/features/reading-list/components/list/utils/utils';
import { useReadingListPage } from '@/features/reading-list/hooks/useReadingListPage';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';

import noBooksImage from '../../assets/images/no-books-found.png';

const ReadingList: FC = () => {
  const {
    missing,
    isFetching,
    totalItems,
    pageSize,
    setPage,
    page,
    statusUI,
    firstName,
    readingList,
  } = useReadingListPage();

  const totalPages = getTotalPages(totalItems, pageSize);

  if (missing) {
    return <p>Invalid reading list request.</p>;
  }

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <div className="reading-list-container">
      <h1 className="read-status-title">{getTitle(statusUI, firstName)}</h1>
      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {readingList.length > 0 ? (
            <>
              {readingList.map((book) => (
                <BookListItem key={book.id} {...book} />
              ))}
              <div className="pagination-container d-flex justify-content-center mt-4">
                <button
                  className="btn pagination-btn"
                  onClick={() => setPage((p) => Math.max(1, p - 1))}
                  disabled={page === 1}
                >
                  <FaArrowLeft /> Previous
                </button>
                <div className="pagination-info mx-3">
                  <span className="current-page">{page}</span> /{' '}
                  <span className="total-pages">{totalPages}</span>
                </div>
                <button
                  className="btn pagination-btn"
                  onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
                  disabled={page === totalPages}
                >
                  Next <FaArrowRight />
                </button>
              </div>
            </>
          ) : (
            <div className="d-flex flex-column align-items-center justify-content-center mt-5">
              <img
                src={noBooksImage}
                alt="No books found"
                className="mb-4"
                style={{ maxWidth: '200px', opacity: 0.7 }}
              />
              <h5 className="text-muted">{firstName} has no books in this list.</h5>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ReadingList;
