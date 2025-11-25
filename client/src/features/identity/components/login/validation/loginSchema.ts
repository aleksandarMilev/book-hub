import type { TFunction } from 'i18next';
import * as Yup from 'yup';

export const createLoginSchema = (t: TFunction<'identity'>) =>
  Yup.object({
    credentials: Yup.string().required(t('login.validation.credentialsRequired')),
    password: Yup.string().required(t('login.validation.passwordRequired')),
    rememberMe: Yup.boolean(),
  });
