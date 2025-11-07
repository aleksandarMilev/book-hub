import { useState, type FC } from 'react';
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa';
import { useLocation } from 'react-router-dom';

import noBooksImage from '../../assets/images/no-books-found.png';
import { pagination, readingListStatus } from '../../common/constants/defaultValues';
import * as hooks from '../../hooks/useReadingList';

import DefaultSpinner from '../common/default-spinner/DefaultSpinner';

import './ReadingList.css';
import BookListItem from '../book/book-list-item/BookListItem';

import type { ReadingStatus } from '../../api/readingList/types/readingList';

const ReadingList: FC = () => {
  const location = useLocation();
  const state = location.state as
    | {
        id: string;
        readingListStatus: string;
        firstName: string;
      }
    | undefined;

  if (!state?.id || !state.readingListStatus || !state.firstName) {
    return <p>Invalid reading list request.</p>;
  }

  const { id, readingListStatus: readStatus, firstName } = state;

  const [page, setPage] = useState<number>(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };

  const { readingList, totalItems, isFetching } = hooks.useList(
    id,
    readStatus as ReadingStatus,
    page,
    pageSize,
    false,
  );

  if (isFetching || !readingList) {
    return <DefaultSpinner />;
  }

  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));

  return (
    <div className="reading-list-container">
      <h1 className="read-status-title">
        {firstName} {readStatus === readingListStatus.read ? 'has read:' : 'wants to read:'}
      </h1>
      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {isFetching ? (
            <DefaultSpinner />
          ) : readingList.length > 0 ? (
            <>
              {readingList.map((book) => (
                <BookListItem key={book.id} {...book} />
              ))}
              <div className="pagination-container d-flex justify-content-center mt-4">
                <button
                  className="btn pagination-btn"
                  onClick={() => handlePageChange(page - 1)}
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
                  onClick={() => handlePageChange(page + 1)}
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
