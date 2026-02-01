import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { chatSchema } from '@/features/chat/components/form/validation/chatSchema';
import { useCreate, useEdit } from '@/features/chat/hooks/useCrud';
import type { Chat, CreateChat } from '@/features/chat/types/chat';
import { routes } from '@/shared/lib/constants/api';
import { useMessage } from '@/shared/stores/message/message';

type Props = {
  chatData?: Chat | null;
  isEditMode?: boolean;
};

export function useChatFormik({ chatData = null, isEditMode = false }: Props) {
  const { t } = useTranslation('chats');

  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const formik = useFormik<CreateChat>({
    initialValues: {
      name: chatData?.name || '',
      image: null,
    },
    validationSchema: chatSchema(t),
    onSubmit: async (values) => {
      try {
        if (isEditMode) {
          const ok = await editHandler(chatData!.id, values);
          if (ok) {
            showMessage(t('form.messages.editSuccess', { name: values.name }), true);
            navigate(`${routes.chat}/${chatData!.id}`);
          }
        } else {
          const created = await createHandler(values);
          if (created?.id) {
            showMessage(t('form.messages.createSuccess', { name: values.name }), true);
            navigate(`${routes.chat}/${created.id}`);
          }
        }
      } catch {
        showMessage(t('form.messages.saveFailed'), false);
      }
    },
  });

  return formik;
}


