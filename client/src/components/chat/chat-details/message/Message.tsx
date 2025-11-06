import type { FC } from 'react';
import { MDBIcon } from 'mdb-react-ui-kit';
import { utcToLocal } from '../../../../common/functions/utils';
import type { Message as MessageModel, Participant } from '../../../../api/chat/types/chat';
import messageImage from '../../../../assets/images/message.webp';

const Message: FC<{
  message: MessageModel;
  isSentByUser: boolean;
  sender?: Participant | undefined;
  onEdit: (message: MessageModel) => void;
  onDelete: (id: number) => void;
  onProfileClick: (id: string) => void;
}> = ({ message, isSentByUser, sender, onEdit, onDelete, onProfileClick }) => {
  const created = utcToLocal(message.createdOn);
  const modified = message.modifiedOn ? utcToLocal(message.modifiedOn) : null;

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
      <img
        src={sender?.imageUrl || messageImage}
        alt="avatar"
        style={{ width: '45px', height: '100%' }}
      />
      <div className="ms-2 profile-item" onClick={() => sender && onProfileClick(sender.id)}>
        <strong>
          {sender?.firstName} {sender?.lastName}
        </strong>
      </div>
    </div>
  );
};

export default Message;
