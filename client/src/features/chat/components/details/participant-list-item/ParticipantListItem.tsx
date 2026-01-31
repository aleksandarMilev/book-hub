import { MDBCardImage, MDBIcon } from 'mdb-react-ui-kit';
import React, { type FC } from 'react';
import { useTranslation } from 'react-i18next';

import type { PrivateProfile } from '@/features/profile/types/profile';
import { getImageUrl } from '@/shared/lib/utils/utils';

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
      className="chat-participant-item d-flex align-items-center mb-3 chat-profile-item"
      onClick={() => onProfileClickHandler(participant.id)}
    >
      <MDBCardImage
        src={getImageUrl(participant.imagePath, 'profiles')}
        alt={participant.firstName}
        className="chat-participant-avatar"
      />

      <span className="chat-participant-name">
        {index === 0 ? (
          <>
            <strong>
              {participant.firstName} {participant.lastName}
            </strong>{' '}
            <span className="text-muted chat-participant-creator">
              {t('participants.creatorSuffix')}
            </span>
          </>
        ) : (
          <>
            {participant.firstName} {participant.lastName}
            {currentUserIsChatCreator && (
              <MDBIcon
                fas
                icon="times"
                className="ms-2 chat-icon-button chat-participant-remove"
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


