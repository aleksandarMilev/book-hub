import { useState } from "react";
import { useLocation } from "react-router-dom";
import { FaArrowLeft, FaArrowRight, FaSearch } from "react-icons/fa";

import { pagination } from "../../../common/constants/defaultValues";
import * as useSearch from "../../../hooks/useSearch";
import * as useBook from "../../../hooks/useBook";

import BookListItem from "../book-list-item/BooksListItem";
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner";

import image from "../../../assets/images/no-books-found.png";

import "./BookList.css";

export default function BookList() {
  const location = useLocation();

  const genreId = location?.state?.genreId;
  const genreName = location?.state?.genreName;

  const authorId = location?.state?.authorId;
  const authorName = location?.state?.authorName;

  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const { books, totalItems, isFetching } = genreId
    ? useBook.useByGenre(genreId, page, pageSize)
    : authorId
    ? useBook.useByAuthor(authorId, page, pageSize)
    : useSearch.useBooks(searchTerm, page, pageSize);

  const totalPages = Math.ceil(totalItems / pageSize);

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setPage(pagination.defaultPageIndex);
  };

  const handlePageChange = (newPage) => {
    setPage(newPage);
  };

  return (
    <div className="container mt-5 mb-5">
      {genreId ? (
        <h1 className="text text-center mb-4">{genreName} Books</h1>
      ) : authorId ? (
        <h1 className="text text-center mb-4">All Books by {authorName}</h1>
      ) : (
        <div className="row mb-4">
          <div className="col-md-10 mx-auto d-flex">
            <div className="search-bar-container d-flex">
              <input
                type="text"
                className="form-control search-input"
                placeholder="Search books..."
                value={searchTerm}
                onChange={handleSearchChange}
              />
              <button
                className="btn btn-light search-btn"
                disabled={isFetching}
              >
                <FaSearch size={20} />
              </button>
            </div>
          </div>
        </div>
      )}
      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {isFetching ? (
            <DefaultSpinner />
          ) : books.length > 0 ? (
            <>
              {books.map((b) => (
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
              <h5 className="text-muted">We couldn't find any books</h5>
              <p
                className="text-muted text-center"
                style={{ maxWidth: "400px" }}
              >
                Try adjusting your search terms or exploring our collection for
                more options.
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
