interface Identifiable {
  id: string | number;
}

interface HasImage {
  imageUrl: string | null;
}

interface Named {
  name: string;
}

interface Timestamped {
  createdOn: string;
  modifiedOn?: string | null;
}

export interface Participant extends Identifiable, HasImage {
  id: string;
  firstName: string;
  lastName: string;
}

export interface ChatSummary extends Identifiable, Named, HasImage {
  id: number;
  creatorId: string;
}

export interface Chat extends ChatSummary {
  participants: Participant[];
  messages: Message[];
}

export type ChatInput = Pick<ChatSummary, 'name' | 'imageUrl'>;

export interface Message extends Identifiable, Timestamped {
  id: number;
  chatId: number;
  senderId: string;
  message: string;
}

export type MessageInput = Pick<Message, 'chatId' | 'message'>;
