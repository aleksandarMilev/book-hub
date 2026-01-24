import * as Yup from 'yup';
import type { TFunction } from 'i18next';

const lengths = {
  minMessage: 1,
  maxMessage: 5_000,
};

export const chatMessageSchema = (t: TFunction<'chats'>) =>
  Yup.object({
    message: Yup.string()
      .min(
        lengths.minMessage,
        t('validation.min', { field: t('sendForm.messageLabel'), min: lengths.minMessage }),
      )
      .max(
        lengths.maxMessage,
        t('validation.max', { field: t('sendForm.messageLabel'), max: lengths.maxMessage }),
      )
      .required(t('validation.required', { field: t('sendForm.messageLabel') })),
  });
