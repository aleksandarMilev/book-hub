import './TopUsers.css';

import { type FC } from 'react';
import { FaBookReader } from 'react-icons/fa';

import { useTopProfilesPage } from '@/features/home/hooks/useTopProfilesPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import EmptyState from '@/shared/components/empty-state/EmptyState.js';
import HomePageError from '@/shared/components/errors/home-page/HomePageError.js';

const TopUsers: FC = () => {
  const { profiles, isFetching, error, onProfileClickHandler } = useTopProfilesPage();

  if (error) {
    return <HomePageError message={error} />;
  }

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (!profiles?.length) {
    return (
      <EmptyState
        icon={<FaBookReader />}
        title="No Profiles Found"
        message="There are no top users available yet."
      />
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
            onClick={() => onProfileClickHandler(p.id)}
            role="button"
            tabIndex={0}
            onKeyDown={(e) => e.key === 'Enter' && onProfileClickHandler(p.id)}
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
