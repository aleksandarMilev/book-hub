import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import type { ChatInput } from '../../../../api/chat/types/chat';
import { routes } from '../../../../common/constants/api';
import { useMessage } from '../../../../contexts/message/messageContext';
import * as hooks from '../../../../hooks/useChat';

import { chatSchema } from '../validation/chatSchema';


export function useChatFormik({
  chatData = null,
  isEditMode = false,
}: {
  chatData?: any;
  isEditMode?: boolean;
}) {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = hooks.useCreate();
  const editHandler = hooks.useEdit();

  const formik = useFormik<ChatInput>({
    initialValues: {
      name: chatData?.name || '',
      imageUrl: chatData?.imageUrl || null,
    },
    validationSchema: chatSchema,
    onSubmit: async (values) => {
      try {
        if (isEditMode) {
          const ok = await editHandler(chatData.id, values);
          if (ok) {
            showMessage(`You have successfully edited ${values.name}`, true);
            navigate(`${routes.chat}/${chatData.id}`);
          }
        } else {
          const chatId = await createHandler(values);
          if (chatId) {
            showMessage(`You have successfully created ${values.name}`, true);
            navigate(`${routes.chat}/${chatId}`);
          }
        }
      } catch {
        showMessage('Something went wrong while creating your chat, please try again!', false);
      }
    },
  });

  return formik;
}
