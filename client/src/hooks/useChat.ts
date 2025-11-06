import { useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import * as api from '../api/chat/chatApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import { useMessage } from '../contexts/message/messageContext';
import type {
  Chat,
  ChatInput,
  ChatSummary,
  MessageInput,
  Participant,
  Message,
} from '../api/chat/types/chat';
import type { Message as MessageModel } from '../api/chat/types/chat';
import { useFormik } from 'formik';
import { chatSchema } from '../components/chat/chat-details/send-form/validation/chatSchema';
import { utcToLocal } from '../common/functions/utils';

export const useChatDetails = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const chatId = useMemo(() => (id ? Number(id) : NaN), [id]);

  const { userId, token } = useContext(UserContext);
  const { showMessage } = useMessage();

  const { chat, isFetching } = useDetails(chatId);

  const [isEditMode, setIsEditMode] = useState(false);
  const [messageToEdit, setMessageToEdit] = useState<MessageModel | null>(null);
  const [messages, setMessages] = useState<MessageModel[]>(chat?.messages || []);

  useEffect(() => {
    setMessages(chat?.messages || []);
  }, [chat]);

  const [participants, setParticipants] = useState<Participant[]>(chat?.participants || []);

  useEffect(() => {
    setParticipants(chat?.participants || []);
  }, [chat]);

  const deleteMessage = useCallback(
    async (messageId: number) => {
      try {
        await removeMessage(messageId, token!);

        setMessages((prev) => prev.filter((m) => m.id !== messageId));
        showMessage('Your message was successfully deleted', true);
      } catch (error: any) {
        showMessage(error?.message ?? 'Failed to delete message.', false);
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
      } catch (error: any) {
        showMessage(error?.message ?? 'Failed to remove user.', false);
      }
    },
    [chatId, token, showMessage],
  );

  const refreshParticipantsList = useCallback((newParticipant: Participant) => {
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
    (message: MessageModel) => {
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
    [navigate, routes.profile, userId],
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
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

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
    } catch (error: any) {
      const message = error?.message ?? 'Failed to delete chat.';
      showMessage(message, false);
    } finally {
      toggleModal();
    }
  }, [id, token, name, navigate, showMessage, showModal, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

export function useChatsNotJoined(userId?: string) {
  const { token } = useContext(UserContext);

  const [chatNames, setChatNames] = useState<ChatSummary[] | null>(null);
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
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to load chat names.';
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

export function useDetails(chatId?: number | string) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [chat, setChat] = useState<Chat | null>(null);
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
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to load chat details.';
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
  const { token } = useContext(UserContext);

  return useCallback(
    async (chatData: ChatInput) => {
      if (!token) {
        return;
      }

      const toSend: ChatInput = { ...chatData, imageUrl: chatData.imageUrl || null };

      try {
        return await api.create(toSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create chat.';
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
}

export function useEdit() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  return useCallback(
    async (chatId: number, chatData: ChatInput) => {
      if (!token) {
        return;
      }

      const toSend: ChatInput = { ...chatData, imageUrl: chatData.imageUrl || null };

      try {
        return await api.edit(chatId, toSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit chat.';
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
}

export function useCreateMessage() {
  const { token } = useContext(UserContext);

  return useCallback(
    async (messageData: MessageInput): Promise<Message> => {
      if (!token) {
        throw new Error('Not authenticated');
      }
      try {
        return await api.createMessage(messageData, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create message.';
        throw new Error(message);
      }
    },
    [token],
  );
}

export function useEditMessage() {
  const { token } = useContext(UserContext);

  return useCallback(
    async (messageId: number, messageData: MessageInput): Promise<Message> => {
      if (!token) {
        throw new Error('Not authenticated');
      }
      try {
        return await api.editMessage(messageId, messageData, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit message.';
        throw new Error(message);
      }
    },
    [token],
  );
}

export function useInviteToChat(refetch?: () => void) {
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

  return useCallback(
    async (chatId: number, userId: string, firstName: string, chatName: string) => {
      if (!token) {
        return;
      }

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

export function useChatButtons(
  chatName = '',
  chatCreatorId?: string,
  refreshParticipantsList?: (p: Participant) => void,
) {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { userId, token } = useContext(UserContext);

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
    } catch (error: any) {
      showMessage(error?.message ?? 'Failed to accept invitation.', false);
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
    } catch (error: any) {
      showMessage(error?.message ?? 'Failed to reject invitation.', false);
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
      } catch (error: any) {
        showMessage(error?.message ?? 'Failed to leave chat.', false);
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
  token,
}: {
  chatId: number;
  isEditMode: boolean;
  messageToEdit: MessageModel | null;
  setIsEditMode: (v: boolean) => void;
  setMessageToEdit: (v: MessageModel | null) => void;
  setMessages: React.Dispatch<React.SetStateAction<MessageModel[]>>;
  token: string;
}) => {
  const { showMessage } = useMessage();

  const createMessage = useCreateMessage();
  const editMessage = useEditMessage();

  const formik = useFormik<{ chatId: number | string; message: string }>({
    initialValues: { chatId, message: '' },
    validationSchema: chatSchema,
    onSubmit: async (values, { resetForm }) => {
      const payload = { chatId, message: values.message };
      try {
        let response: MessageModel;
        if (isEditMode && messageToEdit) {
          response = await editMessage(messageToEdit.id, payload);
          setMessages((prev) =>
            prev.map((m) =>
              m.id === response.id
                ? {
                    ...m,
                    message: response.message,
                    createdOn: utcToLocal(response.createdOn),
                    modifiedOn: response.modifiedOn ? utcToLocal(response.modifiedOn) : null,
                  }
                : m,
            ),
          );
        } else {
          response = await createMessage(payload);
          response.createdOn = utcToLocal(response.createdOn);

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
