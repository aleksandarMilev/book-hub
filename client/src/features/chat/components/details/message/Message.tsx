import { MDBIcon } from 'mdb-react-ui-kit';
import type { FC } from 'react';

import image from '@/features/chat/components/details/message/assets/message.webp';
import type { ChatMessage } from '@/features/chat/types/chat.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';
import { formatIsoDate } from '@/shared/lib/utils.js';

const Message: FC<{
  message: ChatMessage;
  isSentByUser: boolean;
  sender?: PrivateProfile | undefined;
  onEdit: (message: ChatMessage) => void;
  onDelete: (id: number) => void;
  onProfileClick: (id: string) => void;
}> = ({ message, isSentByUser, sender, onEdit, onDelete, onProfileClick }) => {
  const created = formatIsoDate(message.createdOn);
  const modified = message.modifiedOn ? formatIsoDate(message.modifiedOn) : null;

  return (
    <div className={`d-flex flex-row justify-content-${isSentByUser ? 'end' : 'start'} mb-4`}>
      <div
        className={`p-3 ${isSentByUser ? 'me-3 border' : 'ms-3'}`}
        style={{
          borderRadius: '15px',
          backgroundColor: isSentByUser ? '#fbfbfb' : 'rgba(57, 192, 237,.2)',
        }}
      >
        <p className="small mb-0">{message.message}</p>
        <small className="text-muted">{modified ? `${modified} (Modified)` : created}</small>
        {isSentByUser && (
          <>
            <MDBIcon
              fas
              icon="pencil-alt"
              className="ms-2 cursor-pointer"
              onClick={() => onEdit(message)}
            />
            <MDBIcon
              fas
              icon="trash"
              className="ms-2 cursor-pointer"
              onClick={() => onDelete(message.id)}
            />
          </>
        )}
      </div>
      <img src={sender?.imageUrl || image} alt="avatar" style={{ width: '45px', height: '100%' }} />
      <div className="ms-2 profile-item" onClick={() => sender && onProfileClick(sender.id)}>
        <strong>
          {sender?.firstName} {sender?.lastName}
        </strong>
      </div>
    </div>
  );
};

export default Message;
