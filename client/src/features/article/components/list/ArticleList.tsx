import { type FC } from 'react';
import { FaSearch } from 'react-icons/fa';

import { useListPage } from '@/features/article/hooks/useListPage';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';

import image from '../../../assets/images/no-books-found.png';
import ArticleListItem from '../list-item/ArticleListItem';

const ArticleList: FC = () => {
  const {
    articles,
    isFetching,
    searchTerm,
    setSearchTerm,
    page,
    totalPages,
    handlePageChange,
    handleSearchChange,
  } = useListPage();

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
              {articles.map((article) => (
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
