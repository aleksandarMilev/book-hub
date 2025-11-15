import './Statistics.css';

import { type FC } from 'react';
import {
  FaBook,
  FaBookReader,
  FaCommentDots,
  FaNewspaper,
  FaTags,
  FaUsers,
  FaUserTie,
} from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { useStatistics } from '@/features/statistics/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import { useCountUp } from '@/shared/hooks/useCountup.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

const Statistics: FC = () => {
  const { isAuthenticated } = useAuth();
  const { statistics, isFetching, error } = useStatistics();

  const users = useCountUp(statistics?.users ?? 0, 2_000);
  const books = useCountUp(statistics?.books ?? 0, 2_000);
  const authors = useCountUp(statistics?.authors ?? 0, 2_000);
  const reviews = useCountUp(statistics?.reviews ?? 0, 2_000);
  const genres = useCountUp(statistics?.genres ?? 0, 2_000);
  const articles = useCountUp(statistics?.articles ?? 0, 2_000);

  if (isFetching || !statistics) {
    return <DefaultSpinner />;
  }

  if (error) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <div className="text-center">
          <FaBookReader size={100} color="red" className="mb-3" />
          <p className="lead">{error}</p>
        </div>
      </div>
    );
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
            <h4 className="stat-number">{users}</h4>
            <p className="stat-label">users have joined our community</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaBook className="stat-icon" />
            <p className="stat-label">More than</p>
            <h4 className="stat-number">{books}</h4>
            <p className="stat-label">books have been created</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaUserTie className="stat-icon" />
            <p className="stat-label">Over</p>
            <h4 className="stat-number">{authors}</h4>
            <p className="stat-label">authors have been added</p>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaCommentDots className="stat-icon" />
            <p className="stat-label">A total of</p>
            <h4 className="stat-number">{reviews}</h4>
            <p className="stat-label">reviews have been written</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaTags className="stat-icon" />
            <p className="stat-label">Choose from over</p>
            <h4 className="stat-number">{genres}</h4>
            <p className="stat-label">different genres</p>
          </div>
        </div>
        <div className="col-md-4 mb-4">
          <div className="stat-card">
            <FaNewspaper className="stat-icon" />
            <p className="stat-label">Read over</p>
            <h4 className="stat-number">{articles}</h4>
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
