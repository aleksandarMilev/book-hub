import type { Chat } from './chat';
import type { ChatMessage } from './chatMessage';
import type { ChatParticipant } from './chatParticipant';

export interface ChatDetails extends Chat {
  participants: ChatParticipant[];
  messages: ChatMessage[];
}
