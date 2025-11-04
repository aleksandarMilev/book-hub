import type { MessageDisplayProps } from './types/messageDisplayProps.type';
import './Message.css';

export default function MessageDisplay({ message, isSuccess }: MessageDisplayProps) {
  return (
    <div className={`message ${isSuccess ? 'success' : 'error'}`}>
      <p>{message}</p>
    </div>
  );
}
