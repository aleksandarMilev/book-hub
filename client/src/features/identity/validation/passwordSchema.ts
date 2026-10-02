import type { TFunction } from 'i18next';
import * as Yup from 'yup';

// Mirrors the server's production Identity password options (ServiceCollectionExtensions.cs:
// length 8, digit, lowercase, uppercase, no symbol required) and the web models' max length
// (Identity/Shared/Constants.cs). The Unicode classes match .NET's char.IsDigit/IsLower/IsUpper,
// so Cyrillic letters count the same way they do on the server.
export const PASSWORD_MIN_LENGTH = 8;
export const PASSWORD_MAX_LENGTH = 128;

export const createPasswordSchema = (t: TFunction<'identity'>, requiredMessage: string) =>
  Yup.string()
    .required(requiredMessage)
    .min(PASSWORD_MIN_LENGTH, t('password.validation.minLength', { min: PASSWORD_MIN_LENGTH }))
    .max(PASSWORD_MAX_LENGTH, t('password.validation.maxLength', { max: PASSWORD_MAX_LENGTH }))
    .matches(/\p{Nd}/u, t('password.validation.digit'))
    .matches(/\p{Ll}/u, t('password.validation.lowercase'))
    .matches(/\p{Lu}/u, t('password.validation.uppercase'));
