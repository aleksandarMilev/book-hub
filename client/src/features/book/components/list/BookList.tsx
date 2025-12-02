import './BookList.css';

import { type ChangeEvent, type FC, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation } from 'react-router-dom';

import image from '@/assets/images/no-books-found.png';
import BookListItem from '@/features/book/components/list-item/BookListItem.js';
import { useByAuthor, useByGenre } from '@/features/book/hooks/useCrud.js';
import { useSearchBooks } from '@/features/search/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import Pagination from '@/shared/components/pagination/Pagination.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';

const BookList: FC = () => {
  const { t } = useTranslation('books');
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
  const bySearch = useSearchBooks(searchTerm, page, pageSize);

  const isGenreMode = !!genreId;
  const isAuthorMode = !genreId && !!authorId;

  const books = isGenreMode
    ? (byGenre?.books ?? [])
    : isAuthorMode
      ? (byAuthor?.books ?? [])
      : (bySearch?.items ?? []);

  const totalItems = isGenreMode
    ? (byGenre?.totalItems ?? 0)
    : isAuthorMode
      ? (byAuthor?.totalItems ?? 0)
      : (bySearch?.totalItems ?? 0);

  const isFetching = isGenreMode
    ? (byGenre?.isFetching ?? false)
    : isAuthorMode
      ? (byAuthor?.isFetching ?? false)
      : (bySearch?.isFetching ?? false);

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

  const showEmpty = !isFetching && books.length === 0;

  return (
    <div className="book-list-page container">
      {genreId ? (
        <h1 className="text-center mb-4">{genreName} Books</h1>
      ) : authorId ? (
        <h1 className="text-center mb-4">All Books by {authorName}</h1>
      ) : (
        <div className="book-search-wrapper">
          <div className="book-search-bar">
            <input
              type="text"
              className="book-search-input"
              placeholder={t('list.searchPlaceholder')}
              value={searchTerm}
              onChange={handleSearchChange}
              aria-label={t('list.searchPlaceholder')}
            />
          </div>
        </div>
      )}
      <div className="books-container">
        {isFetching && <DefaultSpinner />}
        {!isFetching && !showEmpty && (
          <>
            <div className="books-list">
              {books.map((b) => (
                <BookListItem
                  key={b.id}
                  id={b.id}
                  title={b.title}
                  authorName={b.authorName ?? null}
                  imagePath={b.imagePath ?? ''}
                  shortDescription={b.shortDescription ?? ''}
                  averageRating={b.averageRating ?? 0}
                  genres={b.genres ?? []}
                />
              ))}
            </div>
            <Pagination
              page={page}
              totalPages={totalPages}
              disabled={isFetching}
              onPageChange={handlePageChange}
            />
          </>
        )}
        {showEmpty && (
          <div className="books-empty-state">
            <img src={image} alt={t('list.emptyAlt')} className="books-empty-image" />
            <h4 className="books-empty-title">{t('list.emptyTitle')}</h4>
            <p className="books-empty-message">{t('list.emptyMessage')}</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default BookList;
