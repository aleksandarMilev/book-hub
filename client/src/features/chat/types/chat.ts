import type { PrivateProfile } from '@/features/profile/types/profile.js';

export interface Chat {
  id: string;
  name: string;
  imagePath: string;
  creatorId: string;
}

export interface ChatMessage {
  id: number;
  message: string;
  senderId: string;
  senderName: string;
  senderImagePath: string;
  createdOn: string;
  modifiedOn?: string | null;
}

export interface ChatDetails extends Chat {
  participants: PrivateProfile[];
  messages?: ChatMessage[];
}

export interface CreateChatMessage {
  message: string;
  chatId: string;
}

export interface CreateChat {
  name: string;
  image?: File | null;
}

export interface ChatMessagesQuery {
  before?: number;
  take?: number;
}
