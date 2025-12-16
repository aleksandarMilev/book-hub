import './Statistics.css';

import { type FC } from 'react';
import { FaBook, FaCommentDots, FaNewspaper, FaTags, FaUsers, FaUserTie } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { useStatisticsPage } from '@/features/home/hooks/useStatisticsPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import HomePageError from '@/shared/components/errors/home-page/HomePageError.js';
import { routes } from '@/shared/lib/constants/api.js';

const Statistics: FC = () => {
  const { t, isAuthenticated, isFetching, error, counts } = useStatisticsPage();

  if (error) {
    return <HomePageError message={error} />;
  }

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <div className="statistics-container">
      <h2 className="statistics-title">{t('statistics.title')}</h2>
      <h4>{t('statistics.subtitle')}</h4>
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaUsers className="stat-icon" />
            <p className="stat-label">{t('statistics.cards.users.prefix')}</p>
            <h4 className="stat-number">{counts.users}</h4>
            <p className="stat-label">{t('statistics.cards.users.suffix')}</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaBook className="stat-icon" />
            <p className="stat-label">{t('statistics.cards.books.prefix')}</p>
            <h4 className="stat-number">{counts.books}</h4>
            <p className="stat-label">{t('statistics.cards.books.suffix')}</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaUserTie className="stat-icon" />
            <p className="stat-label">{t('statistics.cards.authors.prefix')}</p>
            <h4 className="stat-number">{counts.authors}</h4>
            <p className="stat-label">{t('statistics.cards.authors.suffix')}</p>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaCommentDots className="stat-icon" />
            <p className="stat-label">{t('statistics.cards.reviews.prefix')}</p>
            <h4 className="stat-number">{counts.reviews}</h4>
            <p className="stat-label">{t('statistics.cards.reviews.suffix')}</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaTags className="stat-icon" />
            <p className="stat-label">{t('statistics.cards.genres.prefix')}</p>
            <h4 className="stat-number">{counts.genres}</h4>
            <p className="stat-label">{t('statistics.cards.genres.suffix')}</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaNewspaper className="stat-icon" />
            <p className="stat-label">{t('statistics.cards.articles.prefix')}</p>
            <h4 className="stat-number">{counts.articles}</h4>
            <p className="stat-label">{t('statistics.cards.articles.suffix')}</p>
          </div>
        </div>
        {!isAuthenticated && (
          <Link to={routes.login} className="link-button">
            {t('statistics.loginCta')}
          </Link>
        )}
      </div>
    </div>
  );
};

export default Statistics;
