import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { http } from '../common/http';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import type {
  Chat,
  ChatInput,
  ChatSummary,
  Message,
  MessageInput,
  Participant,
} from './types/chat';


export async function details(chatId: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}/${chatId}`;
    const { data } = await http.get<Chat>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.details);
    throw error;
  }
}

export async function chatsNotJoined(userId: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chatsNotJoined}/?userId=${userId}`;
    const { data } = await http.get<ChatSummary[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.names);
    throw error;
  }
}

export async function hasAccess(
  chatId: number,
  userId: string,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.chat}/${chatId}/access/${userId}`;
    const { data } = await http.get<boolean>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.details);
    throw error;
  }
}

export async function userIsInvited(
  chatId: number,
  userId: string,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.chat}/${chatId}/invited/${userId}`;
    const { data } = await http.get<boolean>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.details);
    throw error;
  }
}

export async function create(chat: ChatInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}`;
    const { data } = await http.post<number>(url, chat, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.create);
    throw error;
  }
}

export async function edit(chatId: number, chat: ChatInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}/${chatId}`;
    await http.put(url, chat, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.edit);
    throw error;
  }
}

export async function remove(chatId: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}/${chatId}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.delete);
    throw error;
  }
}

export async function inviteUserToChat(
  chatId: number,
  chatName: string,
  userId: string,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.chat}/${chatId}/invite`;
    await http.post(url, { userId, chatName }, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.addUser);
    throw error;
  }
}

export async function reject(
  chatId: number,
  chatName: string,
  chatCreatorId: string,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.rejectChatInvitation}`;
    await http.post(url, { chatId, chatName, chatCreatorId }, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.reject);
    throw error;
  }
}

export async function accept(
  chatId: number,
  chatName: string,
  chatCreatorId: string,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.acceptChatInvitation}`;
    const { data } = await http.post<Participant>(
      url,
      { chatId, chatName, chatCreatorId },
      getAuthConfig(token, signal),
    );

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.accept);
    throw error;
  }
}

export async function removeUser(
  chatId: number,
  userToRemoveId: string,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.removeChatUser}?chatId=${chatId}&userId=${userToRemoveId}`;
    await http.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.removeUser);
    throw error;
  }
}

export async function createMessage(message: MessageInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chatMessage}`;
    const { data } = await http.post<Message>(url, message, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.createMessage);
    throw error;
  }
}

export async function editMessage(
  messageId: number,
  message: MessageInput,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.chatMessage}/${messageId}`;
    const { data } = await http.put<Message>(url, message, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.editMessage);
    throw error;
  }
}

export async function removeMessage(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chatMessage}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.chat.deleteMessage);
    throw error;
  }
}
