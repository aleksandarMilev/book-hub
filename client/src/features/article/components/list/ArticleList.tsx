import './ArticleList.css';

import type { FC } from 'react';
import { FaSearch } from 'react-icons/fa';

import emptyImg from '@/assets/images/no-books-found.png';
import ArticleListItem from '@/features/article/components/list-item/ArticleListItem.js';
import { useListPage } from '@/features/article/hooks/useListPage.js';
import Pagination from '@/shared/components/pagination/Pagination.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';

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
    showEmpty,
  } = useListPage();

  return (
    <div className="article-list-page container">
      <div className="search-wrapper">
        <div className="search-bar">
          <FaSearch className="search-icon" />
          <input
            type="text"
            placeholder="Search articles..."
            value={searchTerm}
            onChange={handleSearchChange}
          />
        </div>
      </div>
      <div className="articles-container">
        {isFetching && <DefaultSpinner />}
        {!isFetching && !showEmpty && (
          <>
            <div className="articles-list">
              {articles.map((article) => (
                <ArticleListItem key={article.id} {...article} />
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
          <div className="empty-state">
            <img src={emptyImg} alt="No articles" onClick={() => setSearchTerm('')} />
            <h4>No articles found</h4>
            <p>Try different keywords or clear your search to explore the full article library.</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default ArticleList;
