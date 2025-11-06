import { MDBCardImage, MDBIcon } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import type { Participant } from '../../../../api/chat/types/chat';

const ParticipantListItem: FC<{
  participant: Participant;
  index: number;
  onProfileClickHandler: (id: string) => void;
  onDeleteHandler: (id: string, firstName: string) => void;
  currentUserIsChatCreator: boolean;
}> = ({ participant, index, onProfileClickHandler, onDeleteHandler, currentUserIsChatCreator }) => {
  return (
    <li
      className="d-flex align-items-center mb-3 profile-item"
      onClick={() => onProfileClickHandler(participant.id)}
    >
      <MDBCardImage
        src={participant.imageUrl || ''}
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
            <span className="text-muted">(Chat Creator)</span>
          </>
        ) : (
          <>
            {participant.firstName} {participant.lastName}
            {currentUserIsChatCreator && (
              <MDBIcon
                fas
                icon="times"
                className="ms-2 cursor-pointer"
                onClick={(e: any) => {
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
