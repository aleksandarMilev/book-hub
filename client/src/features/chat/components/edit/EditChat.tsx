import type { FC } from 'react';
import { useLocation } from 'react-router-dom';

import ChatForm from '@/features/chat/components/form/ChatForm.js';

const EditChat: FC = () => {
  const location = useLocation();
  return <ChatForm chatData={location?.state?.chat} isEditMode />;
};

export default EditChat;
