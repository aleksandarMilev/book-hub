import type {
  Chat,
  ChatDetails,
  ChatMessage,
  ChatMessagesQuery,
  CreateChat,
  CreateChatMessage,
} from '@/features/chat/types/chat.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';
import { getAuthConfig, getAuthConfigForFile, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';

const buildFormData = (chat: CreateChat) => {
  const fd = new FormData();
  fd.append('name', chat.name);

  if (chat.image) {
    fd.append('image', chat.image);
  }

  return fd;
};

export const details = async (chatId: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.chat}/${chatId}`;
    const { data } = await http.get<ChatDetails>(url, getAuthConfig(token, signal));
    return data;
  } catch (error) {
    processError(error, errors.chat.byId);
  }
};

export const messages = async (
  chatId: string,
  token: string,
  query: ChatMessagesQuery = {},
  signal?: AbortSignal,
) => {
  try {
    const params = new URLSearchParams();

    if (query.before != null) {
      params.set('before', String(query.before));
    }

    if (query.take != null) {
      params.set('take', String(query.take));
    }

    const queryString = params.toString();
    const url = `${routes.chat}/${chatId}/messages${queryString ? `?${queryString}` : ''}`;

    const { data } = await http.get<ChatMessage[]>(url, getAuthConfig(token, signal));
    return data;
  } catch (error) {
    processError(error, errors.chat.byId);
  }
};

export const chatsNotJoined = async (userId: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.chatsNotJoined}/?userId=${encodeURIComponent(userId)}`;
    const { data } = await http.get<Chat[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chat.all);
  }
};

export const hasAccess = async (
  chatId: string,
  userId: string,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.chat}/${chatId}/access/${encodeURIComponent(userId)}`;
    const { data } = await http.get<boolean>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chat.byId);
  }
};

export const userIsInvited = async (
  chatId: string,
  userId: string,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.chat}/${chatId}/invited/${encodeURIComponent(userId)}`;
    const { data } = await http.get<boolean>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chat.byId);
  }
};

export const create = async (chat: CreateChat, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.chat}`;
    const formData = buildFormData(chat);

    const { data } = await http.post<ChatDetails>(
      url,
      formData,
      getAuthConfigForFile(token, signal),
    );

    return data;
  } catch (error) {
    processError(error, errors.chat.create);
  }
};

export const edit = async (
  chatId: string,
  chat: CreateChat,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.chat}/${chatId}`;
    const formData = buildFormData(chat);

    await http.put(url, formData, getAuthConfigForFile(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chat.edit);
  }
};

export const remove = async (chatId: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.chat}/${chatId}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chat.delete);
  }
};

export const inviteUserToChat = async (
  chatId: string,
  chatName: string,
  userId: string,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.chat}/${chatId}/invite`;
    await http.post(url, { userId, chatName }, getAuthConfig(token, signal));
    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

export const reject = async (
  chatId: string,
  chatName: string,
  chatCreatorId: string,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.rejectChatInvitation}`;
    await http.post(url, { chatId, chatName, chatCreatorId }, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

export const accept = async (
  chatId: string,
  chatName: string,
  chatCreatorId: string,
  token: string,
  signal?: AbortSignal,
) => {
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
};

export const removeUser = async (
  chatId: string,
  userToRemoveId: string,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.removeChatUser}?chatId=${encodeURIComponent(chatId)}&userId=${encodeURIComponent(
      userToRemoveId,
    )}`;

    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chat.removeUser);
  }
};

export const createMessage = async (
  message: CreateChatMessage,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.chatMessage}`;
    const { data } = await http.post<ChatMessage>(url, message, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chatMessage.create);
  }
};

export const editMessage = async (
  messageId: number,
  message: CreateChatMessage,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.chatMessage}/${messageId}`;
    const { data } = await http.put<ChatMessage>(url, message, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.chatMessage.edit);
  }
};

export const removeMessage = async (id: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.chatMessage}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.chatMessage.delete);
  }
};
