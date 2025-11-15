import './BookList.css';

import { type ChangeEvent, type FC, useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { useLocation } from 'react-router-dom';

import BookListItem from '@/features/book/components/list-item/BookListItem';
import { useByAuthor, useByGenre } from '@/features/book/hooks/useCrud';
import { useSearchBooks } from '@/features/search/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';
import { pagination } from '@/shared/lib/constants/defaultValues';

import image from '../../../assets/images/no-books-found.png';

const BookList: FC = () => {
  const location = useLocation();

  const genreId = location?.state?.genreId;
  const genreName = location?.state?.genreName;

  const authorId = location?.state?.authorId;
  const authorName = location?.state?.authorName;

  const [searchTerm, setSearchTerm] = useState('');
  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const byGenre = useByGenre(genreId, page, pageSize, !!genreId);
  const byAuthor = useByAuthor(authorId, page, pageSize, !genreId && !!authorId);
  const bySearch = useSearchBooks(searchTerm, page, pageSize, !genreId && !authorId);

  const books = byGenre?.books ?? byAuthor?.books ?? bySearch?.items ?? [];
  const totalItems = byGenre?.totalItems ?? byAuthor?.totalItems ?? bySearch?.totalItems ?? 0;
  const isFetching = byGenre?.isFetching ?? byAuthor?.isFetching ?? bySearch?.isFetching ?? false;
  const totalPages = Math.ceil(totalItems / pageSize) || 1;

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    setPage(pagination.defaultPageIndex);
  };

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) {
      return;
    }

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
            <div className="search-bar-container d-flex w-100">
              <input
                type="text"
                className="form-control search-input"
                placeholder="Search books..."
                value={searchTerm}
                onChange={handleSearchChange}
                disabled={isFetching}
                aria-label="Search books"
              />
              <button
                className="btn btn-light search-btn"
                disabled={isFetching}
                aria-label="Search"
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
                <BookListItem
                  key={b.id}
                  id={b.id}
                  title={b.title}
                  authorName={b.authorName ?? 'Unknown Author'}
                  imageUrl={b.imageUrl ?? ''}
                  shortDescription={b.shortDescription ?? ''}
                  averageRating={b.averageRating ?? 0}
                  genres={b.genres ?? []}
                />
              ))}
              <Pagination
                page={page}
                totalPages={totalPages}
                disabled={isFetching}
                onPageChange={handlePageChange}
              />
            </>
          ) : (
            <div className="d-flex flex-column align-items-center justify-content-center mt-5">
              <img
                src={image}
                alt="No books found"
                className="mb-4"
                style={{ maxWidth: '200px', opacity: 0.7 }}
              />
              <h5 className="text-muted">{"We couldn't find any books"}</h5>
              <p className="text-muted text-center" style={{ maxWidth: '400px' }}>
                Try adjusting your search terms or exploring our collection for more options.
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default BookList;
