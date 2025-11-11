import './TopUsers.css';

import { type FC } from 'react';
import { useNavigate } from 'react-router-dom';

import { useTopProfiles } from '@/hooks/useProfile';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

const TopUsers: FC = () => {
  const { userId } = useAuth();
  const navigate = useNavigate();
  const { profiles, isFetching, error } = useTopProfiles();

  if (isFetching || !profiles) {
    return <DefaultSpinner />;
  }

  if (error) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <p className="lead text-danger">Error loading top users. Please try again later.</p>
      </div>
    );
  }

  const onClickHandler = (profileId: string): void => {
    navigate(routes.profile, {
      state: { id: profileId === userId ? null : profileId },
    });
  };

  if (!profiles?.length) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <p className="lead text-muted">No top users found.</p>
      </div>
    );
  }

  return (
    <div className="top-users-container">
      <h2 className="text-center my-4 top-users-title">Top Users</h2>
      <div className="top-users-list">
        {profiles.map((p) => (
          <div
            key={p.id}
            className="top-user-card"
            onClick={() => onClickHandler(p.id)}
            role="button"
            tabIndex={0}
            onKeyDown={(e) => e.key === 'Enter' && onClickHandler(p.id)}
          >
            <img src={p.imageUrl} alt={`${p.firstName} ${p.lastName}`} className="user-image" />
            <div className="user-info">
              <h4>{`${p.firstName} ${p.lastName}`}</h4>
              <p>
                <strong>Books Created:</strong> {p.createdBooksCount}
              </p>
              <p>
                <strong>Authors Created:</strong> {p.createdAuthorsCount}
              </p>
              <p>
                <strong>Reviews Written:</strong> {p.reviewsCount}
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TopUsers;
