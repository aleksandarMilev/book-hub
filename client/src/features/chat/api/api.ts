import type {
  Chat,
  ChatDetails,
  ChatMessage,
  CreateChat,
  CreateChatMessage,
} from '@/features/chat/types/chat.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';

export async function details(chatId: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}/${chatId}`;
    const { data } = await http.get<ChatDetails>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chat.byId);
  }
}

export async function chatsNotJoined(userId: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chatsNotJoined}/?userId=${userId}`;
    const { data } = await http.get<Chat[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chat.all);
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
    processError(error, errors.chat.byId);
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
    processError(error, errors.chat.byId);
  }
}

export async function create(chat: CreateChat, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}`;
    const { data } = await http.post<number>(url, chat, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chat.create);
  }
}

export async function edit(chatId: number, chat: CreateChat, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}/${chatId}`;
    await http.put(url, chat, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chat.edit);
  }
}

export async function remove(chatId: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chat}/${chatId}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chat.delete);
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

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
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

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
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
    const { data } = await http.post<PrivateProfile>(
      url,
      { chatId, chatName, chatCreatorId },
      getAuthConfig(token, signal),
    );

    return data;
  } catch (error) {
    processError(error, baseErrors.general);
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

    return true;
  } catch (error) {
    processError(error, errors.chat.removeUser);
  }
}

export async function createMessage(
  message: CreateChatMessage,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.chatMessage}`;
    const { data } = await http.post<ChatMessage>(url, message, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chatMessage.create);
  }
}

export async function editMessage(
  messageId: number,
  message: CreateChatMessage,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.chatMessage}/${messageId}`;
    const { data } = await http.put<ChatMessage>(url, message, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chatMessage.edit);
  }
}

export async function removeMessage(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.chatMessage}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chatMessage.delete);
  }
}
