import { useFormik } from 'formik';
import type React from 'react';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import * as api from '@/features/chat/api/api.js';
import { chatSchema } from '@/features/chat/components/form/validation/chatSchema.js';
import type {
  Chat,
  ChatDetails,
  ChatMessage,
  CreateChat,
  CreateChatMessage,
} from '@/features/chat/types/chat.js';
import type { PrivateProfile } from '@/features/profile/types/profile.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate, IsCanceledError, IsError } from '@/shared/lib/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useChatDetails = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const chatId = useMemo(() => (id ? Number(id) : NaN), [id]);

  const { userId, token } = useAuth();
  const { showMessage } = useMessage();

  const { chat, isFetching } = useDetails(chatId);

  const [isEditMode, setIsEditMode] = useState(false);
  const [messageToEdit, setMessageToEdit] = useState<ChatMessage | null>(null);
  const [messages, setMessages] = useState<ChatMessage[]>(chat?.messages || []);

  useEffect(() => {
    setMessages(chat?.messages || []);
  }, [chat]);

  const [participants, setParticipants] = useState<PrivateProfile[]>(chat?.participants || []);

  useEffect(() => {
    setParticipants(chat?.participants || []);
  }, [chat]);

  const deleteMessage = useCallback(
    async (messageId: number) => {
      try {
        await removeMessage(messageId, token!);

        setMessages((prev) => prev.filter((m) => m.id !== messageId));
        showMessage('Your message was successfully deleted', true);
      } catch (error) {
        const message = IsError(error) ? error?.message : 'Failed to delete message.';
        showMessage(message, false);
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
        const message = IsError(error) ? error?.message : 'Failed to remove user.';
        showMessage(message, false);
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
  };
};

export const useDeleteChat = (id: number, name?: string) => {
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
      const message = IsError(error) ? error?.message : 'Failed to delete chat.';
      showMessage(message, false);
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

  const fetchData = useCallback(async () => {
    if (!userId || !token) {
      return;
    }

    const controller = new AbortController();

    try {
      setIsFetching(true);

      const data = await api.chatsNotJoined(userId, token, controller.signal);

      setChatNames(data);
      setError(null);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : 'Failed to load chat names.';
      setError(message);
    } finally {
      setIsFetching(false);
    }

    return () => controller.abort();
  }, [userId, token]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { chatNames, isFetching, error, refetch: fetchData };
}

export function useDetails(chatId: number) {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [chat, setChat] = useState<ChatDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const numericId = typeof chatId === 'string' ? Number(chatId) : chatId;

  const fetchData = useCallback(async () => {
    if (!numericId || !token) {
      return;
    }

    const controller = new AbortController();

    try {
      setIsFetching(true);

      const data = await api.details(numericId, token, controller.signal);
      setChat(data);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : 'Failed to load chat details.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }

    return () => controller.abort();
  }, [numericId, token, navigate]);

  useEffect(() => {
    void fetchData();
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

      const toSend: CreateChat = { ...chatData, imageUrl: chatData.imageUrl || null };

      try {
        return await api.create(toSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to create chat.';
        navigate(routes.badRequest, { state: { message } });

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
    async (chatId: number, chatData: CreateChat) => {
      if (!token) {
        return;
      }

      const toSend: CreateChat = { ...chatData, imageUrl: chatData.imageUrl || null };

      try {
        return await api.edit(chatId, toSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to edit chat.';
        navigate(routes.badRequest, { state: { message } });

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
      try {
        return await api.createMessage(messageData, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to create message.';
        throw new Error(message);
      }
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
      try {
        return await api.editMessage(messageId, messageData, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to edit message.';
        throw new Error(message);
      }
    },
    [token],
  );
}

export function useInviteToChat(refetch?: () => void) {
  const { token } = useAuth();
  const { showMessage } = useMessage();

  return useCallback(
    async (chatId: number, userId: string, firstName: string, chatName: string) => {
      if (!token) {
        return;
      }

      try {
        await api.inviteUserToChat(chatId, chatName, userId, token);
        showMessage(`You successfully added ${firstName} to ${chatName}!`, true);
      } catch (error) {
        const message = IsError(error) ? error?.message : 'Failed to invite user.';
        showMessage(message, false);
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
  const navigate = useNavigate();
  const { userId, token } = useAuth();
  const { showMessage } = useMessage();

  const [isInvited, setIsInvited] = useState(false);

  useEffect(() => {
    if (!id || !userId || !token) {
      return;
    }

    api
      .userIsInvited(Number(id), userId, token)
      .then(setIsInvited)
      .catch(() => setIsInvited(false));
  }, [id, userId, token]);

  const onAcceptClick = useCallback(async () => {
    if (!id || !chatCreatorId || !token) {
      return;
    }

    try {
      const newParticipant = await api.accept(Number(id), chatName, chatCreatorId, token);

      showMessage(`You are now a member in ${chatName}!`, true);
      setIsInvited(false);
      refreshParticipantsList?.(newParticipant);
    } catch (error) {
      const message = IsError(error) ? error.message : 'Failed to accept invitation.';
      showMessage(message, false);
    }
  }, [id, chatCreatorId, token, chatName, showMessage, refreshParticipantsList]);

  const onRejectClick = useCallback(async () => {
    if (!id || !chatCreatorId || !token) {
      return;
    }

    try {
      await api.reject(Number(id), chatName, chatCreatorId, token);

      showMessage('You have successfully rejected this chat invitation!', true);
      setIsInvited(false);
      navigate(routes.home);
    } catch (error) {
      const message = IsError(error) ? error.message : 'Failed to accept invitation.';
      showMessage(message, false);
    }
  }, [id, chatCreatorId, token, chatName, showMessage, navigate]);

  const onLeaveClick = useCallback(
    async (profileId: string) => {
      if (!id || !token) {
        return;
      }

      try {
        await api.removeUser(Number(id), profileId, token);

        showMessage('You have successfully left the chat!', true);
        navigate(routes.home);
      } catch (error) {
        const message = IsError(error) ? error?.message : 'Failed to leave chat.';
        showMessage(message, false);
      }
    },
    [id, token, showMessage, navigate],
  );

  return {
    id,
    userId,
    isInvited,
    onAcceptClick,
    onRejectClick,
    onLeaveClick,
  };
}

export const removeMessage = (id: number, token: string) => api.removeMessage(id, token);

export const removeUserFromChat = (chatId: number, userId: string, token: string) =>
  api.removeUser(chatId, userId, token);

export const useSendFormFormik = ({
  chatId,
  isEditMode,
  messageToEdit,
  setIsEditMode,
  setMessageToEdit,
  setMessages,
}: {
  chatId: number;
  isEditMode: boolean;
  messageToEdit: ChatMessage | null;
  setIsEditMode: (value: boolean) => void;
  setMessageToEdit: (v: ChatMessage | null) => void;
  setMessages: React.Dispatch<React.SetStateAction<ChatMessage[]>>;
  token: string;
}) => {
  const { showMessage } = useMessage();

  const createMessage = useCreateMessage();
  const editMessage = useEditMessage();

  const formik = useFormik<{ chatId: number; message: string }>({
    initialValues: { chatId, message: '' },
    validationSchema: chatSchema,
    onSubmit: async (values, { resetForm }) => {
      const payload = { chatId, message: values.message };
      try {
        let response: ChatMessage;
        if (isEditMode && messageToEdit) {
          response = await editMessage(messageToEdit.id, payload);
          setMessages((prev) =>
            prev.map((m) =>
              m.id === response.id
                ? {
                    ...m,
                    message: response.message,
                    createdOn: formatIsoDate(response.createdOn),
                    modifiedOn: response.modifiedOn ? formatIsoDate(response.modifiedOn) : null,
                  }
                : m,
            ),
          );
        } else {
          response = await createMessage(payload);
          response.createdOn = formatIsoDate(response.createdOn);

          setMessages((prev) => [...prev, response]);
        }

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
