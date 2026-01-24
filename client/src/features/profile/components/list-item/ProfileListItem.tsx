import './ProfileListItem.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaLock, FaUnlock } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

type Props = {
  id: string;
  imagePath: string;
  firstName: string;
  lastName: string;
  isPrivate: boolean;
};

const ProfileListItem: FC<Props> = ({
  id: profileId,
  imagePath,
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

  const fullName = `${firstName} ${lastName}`;

  return (
    <div className="profile-list-item">
      <div className="profile-list-item-image-wrap">
        {imagePath ? (
          <img
            className="profile-card-image"
            src={getImageUrl(imagePath, 'profiles')}
            alt={fullName}
            onError={(e) => {
              (e.currentTarget as HTMLImageElement).src =
                'https://via.placeholder.com/160?text=No+Image';
            }}
          />
        ) : (
          <div className="profile-card-placeholder">{t('listItem.noImage')}</div>
        )}
      </div>

      <div className="profile-list-item-content">
        <h5 className="profile-list-item-name" title={fullName}>
          {fullName}
        </h5>

        <div className={`profile-privacy-badge ${isPrivate ? 'is-private' : 'is-public'}`}>
          {isPrivate ? <FaLock className="me-1" /> : <FaUnlock className="me-1" />}
          {isPrivate ? t('listItem.privateProfile') : t('listItem.publicProfile')}
        </div>
      </div>

      <button onClick={onClickHandler} className="profile-list-item-btn">
        {t('listItem.viewProfile')}
      </button>
    </div>
  );
};

export default ProfileListItem;
