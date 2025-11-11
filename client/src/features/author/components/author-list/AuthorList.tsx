import { type ChangeEvent, type FC, useState } from 'react';
import { FaSearch } from 'react-icons/fa';

import image from '@/assets/images/no-books-found.png';
import AuthorListItem from '@/features/author/components/author-list-item/AuthorListItem';
import { useSearchAuthors } from '@/features/search/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';
import { useDebounce } from '@/shared/hooks/useDebounce';
import { pagination } from '@/shared/lib/constants/defaultValues';

const AuthorList: FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const {
    items: authors,
    totalItems,
    isFetching,
  } = useSearchAuthors(debouncedSearchTerm, page, pageSize);

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
      <div className="row mb-4">
        <div className="col-md-10 mx-auto d-flex">
          <div className="search-bar-container d-flex w-100">
            <input
              type="text"
              className="form-control search-input"
              placeholder="Search authors..."
              value={searchTerm}
              onChange={handleSearchChange}
              disabled={isFetching}
            />
            <button className="btn btn-light search-btn" disabled={isFetching}>
              <FaSearch size={20} />
            </button>
          </div>
        </div>
      </div>

      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {isFetching ? (
            <DefaultSpinner />
          ) : authors.length > 0 ? (
            <>
              {authors.map((author) => (
                <AuthorListItem key={author.id} {...author} />
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
                alt="No authors found"
                className="mb-4 clickable"
                style={{ maxWidth: '200px', opacity: 0.7, cursor: 'pointer' }}
                onClick={() => setSearchTerm('')}
              />
              <h5 className="text-muted">{"We couldn't find any authors"}</h5>
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

export default AuthorList;
