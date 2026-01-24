import { MDBIcon } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import fallbackAvatar from '@/features/chat/components/details/message/assets/message.webp';
import type { ChatMessage } from '@/features/chat/types/chat.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';
import { formatIsoDate } from '@/shared/lib/utils/utils.js';

type Props = {
  message: ChatMessage;
  isSentByUser: boolean;
  sender?: PrivateProfile | undefined;
  onEdit: (message: ChatMessage) => void;
  onDelete: (id: number) => void;
  onProfileClick: (id: string) => void;
};

const Message: FC<Props> = ({
  message,
  isSentByUser,
  sender,
  onEdit,
  onDelete,
  onProfileClick,
}) => {
  const { t } = useTranslation('chats');

  const created = formatIsoDate(message.createdOn);
  const modified = message.modifiedOn ? formatIsoDate(message.modifiedOn) : null;
  const displayName = sender ? `${sender.firstName} ${sender.lastName}` : message.senderName;
  const avatar = sender?.imagePath || message.senderImagePath || fallbackAvatar;

  return (
    <div
      className={[
        'chat-message-row',
        'd-flex',
        'flex-row',
        `justify-content-${isSentByUser ? 'end' : 'start'}`,
        'mb-4',
        isSentByUser ? 'chat-message-row--sent' : 'chat-message-row--received',
      ].join(' ')}
    >
      <div
        className={[
          'chat-message-bubble',
          isSentByUser ? 'chat-message-bubble--sent me-3' : 'chat-message-bubble--received ms-3',
        ].join(' ')}
      >
        <p className="chat-message-text small mb-0">{message.message}</p>

        <div className="chat-message-meta">
          <small className="text-muted">
            {modified ? `${modified}${t('message.modifiedSuffix')}` : created}
          </small>

          {isSentByUser && (
            <span className="chat-message-actions">
              <MDBIcon
                fas
                icon="pencil-alt"
                className="chat-icon-button ms-2"
                onClick={() => onEdit(message)}
              />
              <MDBIcon
                fas
                icon="trash"
                className="chat-icon-button ms-2"
                onClick={() => onDelete(message.id)}
              />
            </span>
          )}
        </div>
      </div>

      <img src={avatar} alt={t('message.avatarAlt')} className="chat-message-avatar" />

      <div
        className="ms-2 chat-profile-item chat-message-sender"
        onClick={() => sender && onProfileClick(sender.id)}
        title={displayName}
      >
        <strong>{displayName}</strong>
      </div>
    </div>
  );
};

export default Message;
