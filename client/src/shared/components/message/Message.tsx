import './Message.css';

import type { FC } from 'react';
import { FaCheckCircle, FaTimesCircle } from 'react-icons/fa';

import type { MessageDisplayProps } from '@/shared/components/message/types/messageDisplayProps.type';

const MessageDisplay: FC<MessageDisplayProps> = ({ message, isSuccess }) => {
  return (
    <div className={`message ${isSuccess ? 'success' : 'error'}`}>
      {isSuccess ? (
        <FaCheckCircle size={18} color="#6ee7a0" />
      ) : (
        <FaTimesCircle size={18} color="#ff6b6b" />
      )}
      <p>{message}</p>
    </div>
  );
};

export default MessageDisplay;


