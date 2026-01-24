import { MDBCardImage, MDBIcon } from 'mdb-react-ui-kit';
import React, { type FC } from 'react';
import { useTranslation } from 'react-i18next';

import type { PrivateProfile } from '@/features/profile/types/profile.js';

type Props = {
  participant: PrivateProfile;
  index: number;
  onProfileClickHandler: (id: string) => void;
  onDeleteHandler: (id: string, firstName: string) => void;
  currentUserIsChatCreator: boolean;
};

const ParticipantListItem: FC<Props> = ({
  participant,
  index,
  onProfileClickHandler,
  onDeleteHandler,
  currentUserIsChatCreator,
}) => {
  const { t } = useTranslation('chats');

  return (
    <li
      className="d-flex align-items-center mb-3 profile-item"
      onClick={() => onProfileClickHandler(participant.id)}
    >
      <MDBCardImage
        src={participant.imagePath || ''}
        alt={participant.firstName}
        style={{
          width: '40px',
          height: '40px',
          borderRadius: '50%',
          objectFit: 'cover',
          marginRight: '10px',
        }}
      />
      <span>
        {index === 0 ? (
          <>
            <strong>
              {participant.firstName} {participant.lastName}
            </strong>{' '}
            <span className="text-muted">{t('participants.creatorSuffix')}</span>
          </>
        ) : (
          <>
            {participant.firstName} {participant.lastName}
            {currentUserIsChatCreator && (
              <MDBIcon
                fas
                icon="times"
                className="ms-2 cursor-pointer"
                onClick={(e: React.MouseEvent) => {
                  e.stopPropagation();
                  onDeleteHandler(participant.id, participant.firstName);
                }}
              />
            )}
          </>
        )}
      </span>
    </li>
  );
};

export default ParticipantListItem;
