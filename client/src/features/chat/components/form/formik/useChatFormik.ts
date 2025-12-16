import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { chatSchema } from '@/features/chat/components/form/validation/chatSchema.js';
import { useCreate, useEdit } from '@/features/chat/hooks/useCrud.js';
import type { Chat, CreateChat } from '@/features/chat/types/chat.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useMessage } from '@/shared/stores/message/message.js';

type Props = {
  chatData?: Chat | null;
  isEditMode?: boolean;
};

export function useChatFormik({ chatData = null, isEditMode = false }: Props) {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const formik = useFormik<CreateChat>({
    initialValues: {
      name: chatData?.name || '',
      image: null,
    },
    validationSchema: chatSchema,
    onSubmit: async (values) => {
      try {
        if (isEditMode) {
          const ok = await editHandler(chatData!.id, values);
          if (ok) {
            showMessage(`You have successfully edited ${values.name}`, true);
            navigate(`${routes.chat}/${chatData!.id}`);
          }
        } else {
          const created = await createHandler(values);
          if (created?.id) {
            showMessage(`You have successfully created ${values.name}`, true);
            navigate(`${routes.chat}/${created.id}`);
          }
        }
      } catch {
        showMessage('Something went wrong while saving your chat, please try again!', false);
      }
    },
  });

  return formik;
}
