import { type ChangeEvent, type FC, useState } from 'react';
import { Pagination } from 'react-bootstrap';
import { FaSearch } from 'react-icons/fa';

import type { ArticleSummary } from '@/features/article/types/article';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { pagination } from '@/shared/lib/constants/defaultValues';

import * as hooks from '../../../../hooks/useSearch';
import { useDebounce } from '../../../../shared/hooks/useDebounce';
import image from '../../../assets/images/no-books-found.png';
import ArticleListItem from '../list-item/ArticleListItem';

const ArticleList: FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const {
    items: articles,
    totalItems,
    isFetching,
  } = hooks.useSearchArticles(debouncedSearchTerm, page, pageSize);

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
              placeholder="Search articles..."
              value={searchTerm}
              onChange={handleSearchChange}
              disabled={isFetching}
            />
            <button
              className="btn btn-light search-btn"
              disabled={isFetching}
              aria-label="Search articles"
            >
              <FaSearch size={20} />
            </button>
          </div>
        </div>
      </div>
      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {isFetching ? (
            <DefaultSpinner />
          ) : articles.length > 0 ? (
            <>
              {articles.map((article: ArticleSummary) => (
                <ArticleListItem key={article.id} {...article} />
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
                alt="No articles found"
                className="mb-4 clickable"
                style={{ maxWidth: '200px', opacity: 0.7, cursor: 'pointer' }}
                onClick={() => setSearchTerm('')}
              />
              <h5 className="text-muted">{`We couldn't find any articles`}</h5>
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

export default ArticleList;
