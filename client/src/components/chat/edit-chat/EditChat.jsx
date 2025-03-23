import { useLocation } from "react-router-dom";

import ChatForm from "../chat-form/ChatForm";

export default function EditChat() {
  const location = useLocation();

  return <ChatForm chatData={location?.state?.chat} isEditMode={true} />;
}
