import { type FC } from 'react';
import * as chatHooks from '../../../../hooks/useChat';

const ChatButtons: FC<{
  chatName?: string;
  chatCreatorId?: string;
  refreshParticipantsList: (p: any) => void;
}> = ({ chatName = '', chatCreatorId, refreshParticipantsList }) => {
  const { userId, isInvited, onAcceptClick, onRejectClick, onLeaveClick } =
    chatHooks.useChatButtons(chatName, chatCreatorId, refreshParticipantsList);

  if (isInvited) {
    return (
      <div className="invite-box">
        <p>You were invited to join in {chatName}</p>
        <span className="invite-button accept-button" onClick={onAcceptClick}>
          Accept
        </span>
        <span className="invite-button reject-button" onClick={onRejectClick}>
          Reject
        </span>
      </div>
    );
  }

  return userId === chatCreatorId ? null : (
    <div className="invite-box">
      <span className="invite-button reject-button" onClick={() => onLeaveClick(userId!)}>
        Leave
      </span>
    </div>
  );
};

export default ChatButtons;
