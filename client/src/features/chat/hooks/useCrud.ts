import { useFormik } from 'formik';
import type React from 'react';
import { useCallback, useEffect, useMemo, useRef, useState, useTransition } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import * as api from '@/features/chat/api/api.js';
import type {
  Chat,
  ChatDetails,
  ChatMessage,
  ChatMessagesQuery,
  CreateChat,
  CreateChatMessage,
} from '@/features/chat/types/chat.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

import { chatMessageSchema } from '../components/details/send-form/validation/chatMessageSchema.js';
import { useTranslation } from 'react-i18next';

export const useChatDetails = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  const chatId = useMemo(() => id ?? '', [id]);

  const { userId, token } = useAuth();
  const { showMessage } = useMessage();

  const { chat, isFetching, refetch } = useDetails(chatId);

  const [isEditMode, setIsEditMode] = useState(false);
  const [messageToEdit, setMessageToEdit] = useState<ChatMessage | null>(null);

  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [participants, setParticipants] = useState<PrivateProfile[]>([]);

  const isLoadingMoreRef = useRef(false);

  const normalizeOrder = useCallback((chatMessages: ChatMessage[]) => {
    return chatMessages.slice().sort((leftMessage, rightMessage) => {
      const leftCreatedAtMs = new Date(leftMessage.createdOn).getTime();
      const rightCreatedAtMs = new Date(rightMessage.createdOn).getTime();

      if (leftCreatedAtMs !== rightCreatedAtMs) {
        return leftCreatedAtMs - rightCreatedAtMs;
      }

      return leftMessage.id - rightMessage.id;
    });
  }, []);

  const removeDuplicateMessagesById = useCallback((messagesList: ChatMessage[]) => {
    const messageById = new Map<number, ChatMessage>();

    for (const message of messagesList) {
      messageById.set(message.id, message);
    }

    return Array.from(messageById.values());
  }, []);

  useEffect(() => {
    if (!chat) {
      setParticipants([]);
      setMessages([]);

      return;
    }

    setParticipants(chat.participants || []);

    const fromDetails = chat.messages || [];
    if (fromDetails.length > 0) {
      const withoutDuplicates = removeDuplicateMessagesById(fromDetails);
      const normalized = normalizeOrder(withoutDuplicates);

      setMessages(normalized);
      return;
    }

    if (!chatId || !token) {
      return;
    }

    const controller = new AbortController();

    (async () => {
      try {
        const initial = await api.messages(chatId, token, { take: 20 }, controller.signal);
        const withoutDuplicates = removeDuplicateMessagesById(initial ?? []);
        const normalized = normalizeOrder(withoutDuplicates);

        setMessages(normalized);
      } catch (error) {
        const errorMessage = IsError(error) ? error.message : 'Failed to load messages.';
        showMessage(errorMessage, false);
      }
    })();

    return () => controller.abort();
  }, [chat, chatId, token, normalizeOrder, removeDuplicateMessagesById, showMessage]);

  const loadMoreMessages = useCallback(
    async (take = 50) => {
      if (!chatId || !token) {
        return;
      }

      if (isLoadingMoreRef.current) {
        return;
      }

      isLoadingMoreRef.current = true;

      try {
        const oldestId = messages[0]?.id;

        const query: ChatMessagesQuery = { take };
        if (typeof oldestId === 'number') {
          query.before = oldestId;
        }

        const older = await api.messages(chatId, token, query);
        if (!older?.length) {
          return;
        }

        const merged = [...older, ...messages];
        const deduped = removeDuplicateMessagesById(merged);
        const normalized = normalizeOrder(deduped);

        setMessages(normalized);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const errorMessage = IsError(error) ? error.message : 'Failed to load older messages.';
        showMessage(errorMessage, false);
      } finally {
        isLoadingMoreRef.current = false;
      }
    },
    [chatId, token, messages, normalizeOrder, removeDuplicateMessagesById, showMessage],
  );

  const deleteMessage = useCallback(
    async (messageId: number) => {
      try {
        await removeMessage(messageId, token!);

        setMessages((prev) => prev.filter((m) => m.id !== messageId));
        showMessage('Your message was successfully deleted', true);
      } catch (error) {
        const errorMessage = IsError(error) ? error?.message : 'Failed to delete message.';
        showMessage(errorMessage, false);
      }
    },
    [token, showMessage],
  );

  const removeUserClickHandler = useCallback(
    async (profileId: string, firstName: string) => {
      try {
        await removeUserFromChat(chatId, profileId, token!);

        setParticipants((prev) => prev.filter((p) => p.id !== profileId));
        showMessage(`You have successfully removed ${firstName}!`, true);
      } catch (error) {
        const errorMessage = IsError(error) ? error?.message : 'Failed to remove user.';
        showMessage(errorMessage, false);
      }
    },
    [chatId, token, showMessage],
  );

  const refreshParticipantsList = useCallback((newParticipant: PrivateProfile) => {
    setParticipants((prev) => [...prev, newParticipant]);
  }, []);

  const formik = useSendFormFormik({
    chatId,
    isEditMode,
    messageToEdit,
    setIsEditMode,
    setMessageToEdit,
    setMessages,
    token: token!,
  });

  const handleEditMessage = useCallback(
    (message: ChatMessage) => {
      setIsEditMode(true);
      setMessageToEdit(message);
      formik.setValues({ chatId, message: message.message });
    },
    [formik, chatId],
  );

  const handleCancelEdit = useCallback(() => {
    setIsEditMode(false);
    setMessageToEdit(null);
    formik.resetForm();
  }, [formik]);

  const onProfileClickHandler = useCallback(
    (profileId: string) => {
      navigate(routes.profile, { state: { id: profileId === userId ? null : profileId } });
    },
    [navigate, userId],
  );

  return {
    chatId,
    chat,
    isFetching,
    refetch,
    userId,
    isEditMode,
    messages,
    participants,
    formik,
    handleEditMessage,
    handleCancelEdit,
    deleteMessage,
    removeUserClickHandler,
    refreshParticipantsList,
    onProfileClickHandler,
    loadMoreMessages,
  };
};

export const useDeleteChat = (id: string, name?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!showModal) {
      toggleModal();
      return;
    }

    try {
      await api.remove(id, token!);

      showMessage(`You have successfully deleted ${name || 'this chat'}!`, true);
      navigate(routes.home);
    } catch (error) {
      const errorMessage = IsError(error) ? error?.message : 'Failed to delete chat.';
      showMessage(errorMessage, false);
    } finally {
      toggleModal();
    }
  }, [id, token, name, navigate, showMessage, showModal, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

export function useChatsNotJoined(userId?: string) {
  const { token } = useAuth();

  const [chatNames, setChatNames] = useState<Chat[] | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!userId || !token) {
        return;
      }

      setIsFetching(true);

      try {
        const data = await api.chatsNotJoined(userId, token, signal);
        setChatNames(data ?? null);
        setError(null);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }
        const errorMessage = IsError(error) ? error.message : 'Failed to load chat names.';
        setError(errorMessage);
      } finally {
        setIsFetching(false);
      }
    },
    [userId, token],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  return { chatNames, isFetching, error, refetch: fetchData };
}

export function useDetails(chatId: string) {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [chat, setChat] = useState<ChatDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!chatId || !token) {
        return;
      }

      setIsFetching(true);

      try {
        const data = await api.details(chatId, token, signal);
        setChat(data ?? null);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const errorMessage = IsError(error) ? error.message : 'Failed to load chat details.';
        navigate(routes.badRequest, { state: { message: errorMessage } });
      } finally {
        setIsFetching(false);
      }
    },
    [chatId, token, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  return { chat, isFetching, refetch: fetchData };
}

export function useCreate() {
  const navigate = useNavigate();
  const { token } = useAuth();

  return useCallback(
    async (chatData: CreateChat) => {
      if (!token) {
        return;
      }

      try {
        return await api.create(chatData, token);
      } catch (error) {
        const errorMessage = IsError(error) ? error.message : 'Failed to create chat.';
        navigate(routes.badRequest, { state: { message: errorMessage } });

        throw error;
      }
    },
    [token, navigate],
  );
}

export function useEdit() {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (chatId: string, chatData: CreateChat) => {
      if (!token) {
        return;
      }

      try {
        return await api.edit(chatId, chatData, token);
      } catch (error) {
        const errorMessage = IsError(error) ? error.message : 'Failed to edit chat.';
        navigate(routes.badRequest, { state: { message: errorMessage } });

        throw error;
      }
    },
    [token, navigate],
  );
}

export function useCreateMessage() {
  const { token } = useAuth();

  return useCallback(
    async (messageData: CreateChatMessage): Promise<ChatMessage> => {
      if (!token) {
        throw new Error('Not authenticated');
      }

      const data = await api.createMessage(messageData, token);
      if (!data) {
        throw new Error('Failed to create message.');
      }

      return data;
    },
    [token],
  );
}

export function useEditMessage() {
  const { token } = useAuth();

  return useCallback(
    async (messageId: number, messageData: CreateChatMessage): Promise<ChatMessage> => {
      if (!token) {
        throw new Error('Not authenticated');
      }

      const data = await api.editMessage(messageId, messageData, token);
      if (!data) {
        throw new Error('Failed to edit message.');
      }

      return data;
    },
    [token],
  );
}

export function useInviteToChat(refetch?: () => void) {
  const { token } = useAuth();
  const { showMessage } = useMessage();

  return useCallback(
    async (chatId: string, userId: string, firstName: string, chatName: string) => {
      if (!token) {
        return;
      }

      try {
        await api.inviteUserToChat(chatId, chatName, userId, token);
        showMessage(`You successfully added ${firstName} to ${chatName}!`, true);
      } catch (error) {
        const errorMessage = IsError(error) ? error?.message : 'Failed to invite user.';
        showMessage(errorMessage, false);
      } finally {
        refetch?.();
      }
    },
    [token, refetch, showMessage],
  );
}

export function useChatButtons(
  chatName = '',
  chatCreatorId?: string,
  refreshParticipantsList?: (participant: PrivateProfile) => void,
) {
  const { id } = useParams<{ id: string }>();
  const chatId = id ?? '';

  const navigate = useNavigate();
  const { userId, token } = useAuth();
  const { showMessage } = useMessage();

  const [isInvited, setIsInvited] = useState(false);

  useEffect(() => {
    if (!chatId || !userId || !token) {
      return;
    }

    api
      .userIsInvited(chatId, userId, token)
      .then((v) => setIsInvited(Boolean(v)))
      .catch(() => setIsInvited(false));
  }, [chatId, userId, token]);

  const onAcceptClick = useCallback(async () => {
    if (!chatId || !chatCreatorId || !token) {
      return;
    }

    try {
      const newParticipant = await api.accept(chatId, chatName, chatCreatorId, token);

      if (newParticipant) {
        showMessage(`You are now a member in ${chatName}!`, true);
        setIsInvited(false);
        refreshParticipantsList?.(newParticipant);
      }
    } catch (error) {
      const errorMessage = IsError(error) ? error.message : 'Failed to accept invitation.';
      showMessage(errorMessage, false);
    }
  }, [chatId, chatCreatorId, token, chatName, showMessage, refreshParticipantsList]);

  const onRejectClick = useCallback(async () => {
    if (!chatId || !chatCreatorId || !token) {
      return;
    }

    try {
      await api.reject(chatId, chatName, chatCreatorId, token);

      showMessage('You have successfully rejected this chat invitation!', true);
      setIsInvited(false);
      navigate(routes.home);
    } catch (error) {
      const errorMessage = IsError(error) ? error.message : 'Failed to reject invitation.';
      showMessage(errorMessage, false);
    }
  }, [chatId, chatCreatorId, token, chatName, showMessage, navigate]);

  const onLeaveClick = useCallback(
    async (profileId: string) => {
      if (!chatId || !token) {
        return;
      }

      try {
        await api.removeUser(chatId, profileId, token);

        showMessage('You have successfully left the chat!', true);
        navigate(routes.home);
      } catch (error) {
        const errorMessage = IsError(error) ? error?.message : 'Failed to leave chat.';
        showMessage(errorMessage, false);
      }
    },
    [chatId, token, showMessage, navigate],
  );

  return {
    id: chatId,
    userId,
    isInvited,
    onAcceptClick,
    onRejectClick,
    onLeaveClick,
  };
}

export const removeMessage = (id: number, token: string) => api.removeMessage(id, token);

export const removeUserFromChat = (chatId: string, userId: string, token: string) =>
  api.removeUser(chatId, userId, token);

export const useSendFormFormik = ({
  chatId,
  isEditMode,
  messageToEdit,
  setIsEditMode,
  setMessageToEdit,
  setMessages,
}: {
  chatId: string;
  isEditMode: boolean;
  messageToEdit: ChatMessage | null;
  setIsEditMode: (value: boolean) => void;
  setMessageToEdit: (value: ChatMessage | null) => void;
  setMessages: React.Dispatch<React.SetStateAction<ChatMessage[]>>;
  token: string;
}) => {
  const { t } = useTranslation('chats');
  const { showMessage } = useMessage();

  const createMessage = useCreateMessage();
  const updateMessage = useEditMessage();

  const sortMessagesChronologically = (messagesList: ChatMessage[]) => {
    return messagesList.slice().sort((leftMessage, rightMessage) => {
      const leftCreatedAtMs = new Date(leftMessage.createdOn).getTime();
      const rightCreatedAtMs = new Date(rightMessage.createdOn).getTime();

      if (leftCreatedAtMs !== rightCreatedAtMs) {
        return leftCreatedAtMs - rightCreatedAtMs;
      }

      return leftMessage.id - rightMessage.id;
    });
  };

  const uniqueMessagesById = (messagesList: ChatMessage[]) => {
    const messageById = new Map<number, ChatMessage>();

    for (const message of messagesList) {
      messageById.set(message.id, message);
    }

    return Array.from(messageById.values());
  };

  const mergeMessageThenNormalize = (currentMessages: ChatMessage[], nextMessage: ChatMessage) => {
    const messagesWithoutOldVersion = currentMessages.filter(
      (message) => message.id !== nextMessage.id,
    );

    const mergedMessages = [...messagesWithoutOldVersion, nextMessage];
    return sortMessagesChronologically(uniqueMessagesById(mergedMessages));
  };

  const formik = useFormik<{ chatId: string; message: string }>({
    enableReinitialize: true,
    initialValues: { chatId, message: '' },
    validationSchema: chatMessageSchema(t),
    onSubmit: async (formValues, { resetForm }) => {
      const messagePayload: CreateChatMessage = {
        chatId: formValues.chatId,
        message: formValues.message,
      };

      try {
        const savedMessage =
          isEditMode && messageToEdit
            ? await updateMessage(messageToEdit.id, messagePayload)
            : await createMessage(messagePayload);

        setMessages((currentMessages) => mergeMessageThenNormalize(currentMessages, savedMessage));
        resetForm();
      } catch {
        showMessage('Something went wrong while processing your message, please try again', false);
      } finally {
        setIsEditMode(false);
        setMessageToEdit(null);
      }
    },
  });

  return formik;
};
