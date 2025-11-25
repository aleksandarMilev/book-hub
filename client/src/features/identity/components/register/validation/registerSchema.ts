import type { TFunction } from 'i18next';
import * as Yup from 'yup';

export const createRegisterSchema = (t: TFunction<'identity'>) =>
  Yup.object({
    username: Yup.string().required(t('register.validation.usernameRequired')),
    email: Yup.string()
      .email(t('register.validation.emailInvalid'))
      .required(t('register.validation.emailRequired')),
    password: Yup.string().required(t('register.validation.passwordRequired')),
    confirmPassword: Yup.string()
      .oneOf([Yup.ref('password'), undefined], t('register.validation.passwordsMustMatch'))
      .required(t('register.validation.confirmPasswordRequired')),
  });
