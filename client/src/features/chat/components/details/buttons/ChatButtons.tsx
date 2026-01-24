import { type FC } from 'react';
import { useTranslation } from 'react-i18next';

import { useChatButtons } from '@/features/chat/hooks/useCrud.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';

const ChatButtons: FC<{
  chatName?: string;
  chatCreatorId?: string;
  refreshParticipantsList: (p: PrivateProfile) => void;
}> = ({ chatName = '', chatCreatorId, refreshParticipantsList }) => {
  const { t } = useTranslation('chats');

  const { userId, isInvited, onAcceptClick, onRejectClick, onLeaveClick } = useChatButtons(
    chatName,
    chatCreatorId,
    refreshParticipantsList,
  );

  if (isInvited) {
    return (
      <div className="chat-invite-box">
        <p className="chat-invite-text">{t('buttons.invite.message', { chatName })}</p>
        <span className="chat-invite-button chat-invite-button--accept" onClick={onAcceptClick}>
          {t('buttons.invite.accept')}
        </span>
        <span className="chat-invite-button chat-invite-button--reject" onClick={onRejectClick}>
          {t('buttons.invite.reject')}
        </span>
      </div>
    );
  }

  return userId === chatCreatorId ? null : (
    <div className="chat-invite-box">
      <span
        className="chat-invite-button chat-invite-button--reject"
        onClick={() => onLeaveClick(userId!)}
      >
        {t('buttons.leave')}
      </span>
    </div>
  );
};

export default ChatButtons;
