import type { PrivateProfile } from '@/features/profile/types/profile.js';

export interface Chat {
  id: number;
  name: string;
  imageUrl: string;
  creatorId: string;
}

export interface ChatMessage {
  id: number;
  message: string;
  senderId: string;
  senderName: string;
  senderImageUrl: string;
  createdOn: string;
  modifiedOn?: string | null;
}

export interface ChatDetails extends Chat {
  participants: PrivateProfile[];
  messages: ChatMessage[];
}

export interface CreateChatMessage {
  message: string;
  chatId: number;
}

export interface CreateChat {
  name: string;
  imageUrl?: string | null;
}
