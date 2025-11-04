export interface ChatMessage {
  id: number;
  message: string;
  senderId: string;
  senderName: string;
  senderImageUrl: string;
  createdOn: string;
  modifiedOn?: string | null;
}
