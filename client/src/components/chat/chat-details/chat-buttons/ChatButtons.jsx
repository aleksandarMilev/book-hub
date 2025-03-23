import { useState, useEffect, useContext } from "react";
import { useNavigate, useParams } from "react-router-dom";

import * as api from "../../../../api/chatApi";
import { UserContext } from "../../../../contexts/userContext";
import { useMessage } from "../../../../contexts/messageContext";
import { routes } from "../../../../common/constants/api";

export default function ChatButtons({
  chatName,
  chatCreatorId,
  refreshParticipantsList,
}) {
  const { id } = useParams();
  const navigate = useNavigate();

  const { userId, token } = useContext(UserContext);
  const { showMessage } = useMessage();

  const [isInvited, setIsinvited] = useState(false);

  useEffect(() => {
    api.userIsInvitedAsync(id, userId, token).then((r) => setIsinvited(r));
  }, [id, userId, token]);

  const onAcceptClick = async () => {
    try {
      const newParticipant = await api.acceptAsync(
        id,
        chatName,
        chatCreatorId,
        token
      );
      showMessage(`You are now a member in ${chatName}!`, true);
      setIsinvited(false);
      refreshParticipantsList(newParticipant);
    } catch (error) {
      showMessage(error.message, false);
    }
  };

  const onRejectClick = async () => {
    try {
      await api.rejectAsync(id, chatName, chatCreatorId, token);
      showMessage("You have successfully rejected this chat invitation!", true);
      setIsinvited(false);
      navigate(routes.home);
    } catch (error) {
      showMessage(error.message, false);
    }
  };

  const onLeaveClick = async (profileId) => {
    try {
      await api.removeUserAsync(id, profileId, token);
      showMessage(`You have successfuly left the chat!`, true);
      navigate(routes.home);
    } catch (error) {
      showMessage(error.message, false);
    }
  };

  return isInvited ? (
    <div className="invite-box">
      <p>You were invited to join in {chatName}</p>
      <span className="invite-button accept-button" onClick={onAcceptClick}>
        Accept
      </span>
      <span className="invite-button reject-button" onClick={onRejectClick}>
        Reject
      </span>
    </div>
  ) : (
    userId === chatCreatorId || (
      <div className="invite-box">
        <span
          className="invite-button reject-button"
          onClick={() => onLeaveClick(userId)}
        >
          Leave
        </span>
      </div>
    )
  );
}
