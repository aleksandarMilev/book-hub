import './ProfileListItem.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaLock, FaUnlock, FaUser } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

type Props = {
  id: string;
  imageUrl: string;
  firstName: string;
  lastName: string;
  isPrivate: boolean;
};

const ProfileListItem: FC<Props> = ({
  id: profileId,
  imageUrl,
  firstName,
  lastName,
  isPrivate,
}) => {
  const { userId } = useAuth();
  const navigate = useNavigate();
  const { t } = useTranslation('profiles');

  const onClickHandler = () => {
    navigate(routes.profile, {
      state: { id: profileId === userId ? null : profileId },
    });
  };

  return (
    <div className="row p-3 bg-light border rounded mb-3 shadow-sm profile-list-item">
      <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
        {imageUrl ? (
          <img
            className="img-fluid img-responsive rounded-circle profile-list-item-image"
            src={imageUrl}
            alt={`${firstName} ${lastName}`}
            onError={(e) => {
              (e.currentTarget as HTMLImageElement).src =
                'https://via.placeholder.com/160?text=No+Image';
            }}
          />
        ) : (
          <div className="profile-list-item-placeholder rounded-circle">
            {t('listItem.noImage')}
          </div>
        )}
      </div>
      <div className="col-md-6 col-8 mt-1 profile-list-item-content">
        <h5 className="mb-2 profile-list-item-name">
          <FaUser className="me-2" />
          {firstName} {lastName}
        </h5>
        <h6 className="text-muted mb-2 profile-list-item-privacy">
          {isPrivate ? (
            <>
              <FaLock className="me-2" />
              {t('listItem.privateProfile')}
            </>
          ) : (
            <>
              <FaUnlock className="me-2" />
              {t('listItem.publicProfile')}
            </>
          )}
        </h6>
      </div>
      <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
        <div className="d-flex flex-column align-items-center">
          <button onClick={onClickHandler} className="btn btn-sm btn-primary profile-list-item-btn">
            {t('listItem.viewProfile')}
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProfileListItem;
