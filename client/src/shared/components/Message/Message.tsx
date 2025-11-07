import './Message.css';

import type { MessageDisplayProps } from '@/shared/components/message/types/messageDisplayProps.type';

export default function MessageDisplay({ message, isSuccess }: MessageDisplayProps) {
  return (
    <div className={`message ${isSuccess ? 'success' : 'error'}`}>
      <p>{message}</p>
    </div>
  );
}
