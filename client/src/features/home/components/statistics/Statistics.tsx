import './Statistics.css';

import { type FC } from 'react';
import { FaBook, FaCommentDots, FaNewspaper, FaTags, FaUsers, FaUserTie } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { useStatisticsPage } from '@/features/home/hooks/useStatisticsPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import HomePageError from '@/shared/components/errors/home-page/HomePageError.js';
import { routes } from '@/shared/lib/constants/api.js';

const Statistics: FC = () => {
  const { isAuthenticated, isFetching, error, counts } = useStatisticsPage();

  if (error) {
    return <HomePageError message={error} />;
  }

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <div className="statistics-container">
      <h2 className="statistics-title">Welcome to BookHub!</h2>
      <h4>Dive Into a Universe of Books and Beyond</h4>
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaUsers className="stat-icon" />
            <p className="stat-label">Over</p>
            <h4 className="stat-number">{counts.users}</h4>
            <p className="stat-label">users have joined our community</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaBook className="stat-icon" />
            <p className="stat-label">More than</p>
            <h4 className="stat-number">{counts.books}</h4>
            <p className="stat-label">books have been created</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaUserTie className="stat-icon" />
            <p className="stat-label">Over</p>
            <h4 className="stat-number">{counts.authors}</h4>
            <p className="stat-label">authors have been added</p>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaCommentDots className="stat-icon" />
            <p className="stat-label">A total of</p>
            <h4 className="stat-number">{counts.reviews}</h4>
            <p className="stat-label">reviews have been written</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaTags className="stat-icon" />
            <p className="stat-label">Choose from over</p>
            <h4 className="stat-number">{counts.genres}</h4>
            <p className="stat-label">different genres</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaNewspaper className="stat-icon" />
            <p className="stat-label">Read over</p>
            <h4 className="stat-number">{counts.articles}</h4>
            <p className="stat-label">articles on various topics</p>
          </div>
        </div>
        {!isAuthenticated && (
          <Link to={routes.login} className="link-button">
            Join Now!
          </Link>
        )}
      </div>
    </div>
  );
};

export default Statistics;
