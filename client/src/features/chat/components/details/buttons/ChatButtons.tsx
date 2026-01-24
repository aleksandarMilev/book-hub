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
      <div className="invite-box">
        <p>{t('buttons.invite.message', { chatName })}</p>
        <span className="invite-button accept-button" onClick={onAcceptClick}>
          {t('buttons.invite.accept')}
        </span>
        <span className="invite-button reject-button" onClick={onRejectClick}>
          {t('buttons.invite.reject')}
        </span>
      </div>
    );
  }

  return userId === chatCreatorId ? null : (
    <div className="invite-box">
      <span className="invite-button reject-button" onClick={() => onLeaveClick(userId!)}>
        {t('buttons.leave')}
      </span>
    </div>
  );
};

export default ChatButtons;
