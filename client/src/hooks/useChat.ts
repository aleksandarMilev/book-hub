import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import * as api from '../api/chat/chatApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { Chat } from '../api/chat/types/chat';
import type { ChatDetails } from '../api/chat/types/chatDetails';
import type { ChatMessage } from '../api/chat/types/chatMessage';
import { useMessage } from '../contexts/message/messageContext';

export function useChatsNotJoined(userId?: string) {
  const { token } = useContext(UserContext);

  const [chatNames, setChatNames] = useState<Chat[] | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!userId || !token) {
      return;
    }

    try {
      setIsFetching(true);
      const data = await api.chatsNotJoined(userId, token);
      setChatNames(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load chat names.';
      setError(message);
    } finally {
      setIsFetching(false);
    }
  }, [userId, token]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { chatNames, isFetching, error, refetch: fetchData };
}

export function useDetails(chatId: number) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [chat, setChat] = useState<ChatDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!chatId || !token) {
      return;
    }

    try {
      setIsFetching(true);
      const data = await api.details(chatId, token);
      setChat(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load chat details.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [chatId, token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { chat, isFetching, refetch: fetchData };
}

export function useCreate() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (chatData: Partial<Chat>) => {
      const chatToSend: Partial<Chat> = {
        ...chatData,
        imageUrl: chatData.imageUrl || null,
      };

      try {
        return await api.create(chatToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create chat.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return createHandler;
}

export function useEdit() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (chatId: number, chatData: Partial<Chat>) => {
      const chatToSend: Partial<Chat> = {
        ...chatData,
        imageUrl: chatData.imageUrl || null,
      };

      try {
        return await api.edit(chatId, chatToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit chat.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

export function useCreateMessage() {
  const { token } = useContext(UserContext);

  const createHandler = useCallback(
    async (messageData: Partial<ChatMessage>) => {
      try {
        return await api.createMessage(messageData, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create message.';
        throw new Error(message);
      }
    },
    [token],
  );

  return createHandler;
}

export function useEditMessage() {
  const { token } = useContext(UserContext);

  const editHandler = useCallback(
    async (
      messageId: number,
      messageData: Partial<ChatMessage>,
    ): Promise<ChatMessage | undefined> => {
      try {
        return await api.editMessage(messageId, messageData, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit message.';
        throw new Error(message);
      }
    },
    [token],
  );

  return editHandler;
}

export function useInviteToChat(refetch?: () => void) {
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

  return useCallback(
    async (chatId: number, userId: string, firstName: string, chatName: string) => {
      try {
        await api.inviteUserToChat(chatId, chatName, userId, token);
        showMessage(`You successfully added ${firstName} to ${chatName}!`, true);
      } catch (err: any) {
        showMessage(err?.message ?? 'Failed to invite user.', false);
      } finally {
        refetch?.();
      }
    },
    [token, refetch, showMessage],
  );
}
