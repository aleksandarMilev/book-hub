import './ArticleList.css';

import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaSearch } from 'react-icons/fa';

import emptyImg from '@/assets/images/no-books-found.png';
import ArticleListItem from '@/features/article/components/list-item/ArticleListItem.js';
import { useListPage } from '@/features/article/hooks/useListPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import Pagination from '@/shared/components/pagination/Pagination.js';

const ArticleList: FC = () => {
  const { t } = useTranslation('articles');
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
            placeholder={t('list.searchPlaceholder')}
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
            <img src={emptyImg} alt={t('list.emptyAlt')} onClick={() => setSearchTerm('')} />
            <h4>{t('list.emptyTitle')}</h4>
            <p>{t('list.emptyMessage')}</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default ArticleList;
