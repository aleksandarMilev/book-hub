import { useState } from "react";
import { useLocation } from "react-router-dom";
import { FaArrowLeft, FaArrowRight } from "react-icons/fa";

import * as useReadingList from "../../hooks/useReadingList";
import { pagination } from "../../common/constants/defaultValues";
import { readingListStatus } from "../../common/constants/defaultValues";

import BookListItem from "../../components/book/book-list-item/BooksListItem";
import DefaultSpinner from "../common/default-spinner/DefaultSpinner";

import image from "../../assets/images/no-books-found.png";

import "./ReadingList.css";

export default function ReadingList() {
  const location = useLocation();

  const { id } = location?.state;
  const { readingListStatus: readStatus } = location?.state;
  const { firstName } = location?.state;

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;
  const handlePageChange = (newPage) => {
    setPage(newPage);
  };

  const { readingList, totalItems, isFetching } = useReadingList.useGet(
    id,
    readStatus,
    page,
    pageSize,
    false
  );

  if (isFetching || !readingList) {
    return <DefaultSpinner />;
  }

  const totalPages = Math.ceil(totalItems / pageSize);

  return (
    <>
      <h1 className="read-status-title">
        {firstName}{" "}
        {readStatus === readingListStatus.read
          ? " have read: "
          : " wants to read: "}
      </h1>
      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {isFetching ? (
            <DefaultSpinner />
          ) : readingList.length > 0 ? (
            <>
              {readingList.map((b) => (
                <BookListItem key={b.id} {...b} />
              ))}
              <div className="pagination-container d-flex justify-content-center mt-4">
                <button
                  className={`btn pagination-btn ${
                    page === 1 ? "disabled" : ""
                  }`}
                  onClick={() => handlePageChange(page - 1)}
                  disabled={page === 1}
                >
                  <FaArrowLeft /> Previous
                </button>
                <div className="pagination-info">
                  <span className="current-page">{page}</span> /{" "}
                  <span className="total-pages">{totalPages}</span>
                </div>
                <button
                  className={`btn pagination-btn ${
                    page === totalPages ? "disabled" : ""
                  }`}
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
                src={image}
                alt="No books found"
                className="mb-4"
                style={{ maxWidth: "200px", opacity: 0.7 }}
              />
              <h5 className="text-muted">
                {firstName} has not any books in this list.
              </h5>
            </div>
          )}
        </div>
      </div>
    </>
  );
}
