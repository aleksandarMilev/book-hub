import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig } from '../common/utils';
import type { ChatDetails } from './types/chatDetails';
import type { Chat } from './types/chat';
import type { ChatMessage } from './types/chatMessage';

export async function details(chatId: number, token: string) {
  try {
    const url = `${baseUrl}${routes.chat}/${chatId}`;
    const response = await axios.get<ChatDetails>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.details);
  }
}

export async function chatsNotJoined(userId: string, token: string) {
  try {
    const url = `${baseUrl}${routes.chatsNotJoined}/?userId=${userId}`;
    const response = await axios.get<Chat[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.names);
  }
}

export async function hasAccess(chatId: number, userId: string, token: string): Promise<boolean> {
  try {
    const url = `${baseUrl}${routes.chat}/${chatId}/access/${userId}`;
    const response = await axios.get<boolean>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.details);
  }
}

export async function userIsInvited(chatId: number, userId: string, token: string) {
  try {
    const url = `${baseUrl}${routes.chat}/${chatId}/invited/${userId}`;
    const response = await axios.get<boolean>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.details);
  }
}

export async function create(chat: Partial<Chat>, token: string) {
  try {
    const url = `${baseUrl}${routes.chat}`;
    const response = await axios.post<number>(url, chat, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.create);
  }
}

export async function edit(chatId: number, chat: Partial<Chat>, token: string): Promise<boolean> {
  try {
    const url = `${baseUrl}${routes.chat}/${chatId}`;
    await axios.put(url, chat, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.chat.edit);
  }
}

export async function remove(chatId: number, token: string): Promise<boolean> {
  try {
    const url = `${baseUrl}${routes.chat}/${chatId}`;
    await axios.delete(url, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.chat.delete);
  }
}

export async function inviteUserToChat(
  chatId: number,
  chatName: string,
  userId: string,
  token: string,
) {
  try {
    const url = `${baseUrl}${routes.chat}/${chatId}/invite`;
    await axios.post(url, { userId, chatName }, getAuthConfig(token));
  } catch {
    throw new Error(errors.chat.addUser);
  }
}

export async function reject(
  chatId: number,
  chatName: string,
  chatCreatorId: string,
  token: string,
) {
  try {
    const url = `${baseUrl}${routes.rejectChatInvitation}`;
    await axios.post(url, { chatId, chatName, chatCreatorId }, getAuthConfig(token));
  } catch {
    throw new Error(errors.chat.reject);
  }
}

export async function accept(
  chatId: number,
  chatName: string,
  chatCreatorId: string,
  token: string,
) {
  try {
    const url = `${baseUrl}${routes.acceptChatInvitation}`;
    const response = await axios.post(
      url,
      { chatId, chatName, chatCreatorId },
      getAuthConfig(token),
    );

    return response.data;
  } catch {
    throw new Error(errors.chat.accept);
  }
}

export async function removeUser(chatId: number, userToRemoveId: string, token: string) {
  try {
    const url = `${baseUrl}${routes.removeChatUser}?chatId=${chatId}&userId=${userToRemoveId}`;
    await axios.delete(url, getAuthConfig(token));
  } catch {
    throw new Error(errors.chat.removeUser);
  }
}

export async function createMessage(message: Partial<ChatMessage>, token: string) {
  try {
    const url = `${baseUrl}${routes.chatMessage}`;
    const response = await axios.post<ChatMessage>(url, message, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.createMessage);
  }
}

export async function editMessage(messageId: number, message: Partial<ChatMessage>, token: string) {
  try {
    const url = `${baseUrl}${routes.chatMessage}/${messageId}`;
    const response = await axios.put<ChatMessage>(url, message, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.chat.editMessage);
  }
}

export async function removeMessage(id: number, token: string): Promise<void> {
  try {
    const url = `${baseUrl}${routes.chatMessage}/${id}`;
    await axios.delete(url, getAuthConfig(token));
  } catch {
    throw new Error(errors.chat.deleteMessage);
  }
}
