import type { FC } from 'react';
import { useLocation } from 'react-router-dom';

import ChatForm from '../chat-form/ChatForm';


const EditChat: FC = () => {
  const location = useLocation();
  return <ChatForm chatData={location?.state?.chat} isEditMode />;
};

export default EditChat;
