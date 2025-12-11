import type { TFunction } from 'i18next';
import * as Yup from 'yup';

const NAME_MIN = 2;
const NAME_MAX = 100;

const URL_MIN = 10;
const URL_MAX = 2_000;

const BIO_MIN = 10;
const BIO_MAX = 1_000;

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024; // 2MB
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

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

    firstName: Yup.string()
      .min(NAME_MIN, t('register.validation.firstNameMin', { min: NAME_MIN }))
      .max(NAME_MAX, t('register.validation.firstNameMax', { max: NAME_MAX }))
      .required(t('register.validation.firstNameRequired')),

    lastName: Yup.string()
      .min(NAME_MIN, t('register.validation.lastNameMin', { min: NAME_MIN }))
      .max(NAME_MAX, t('register.validation.lastNameMax', { max: NAME_MAX }))
      .required(t('register.validation.lastNameRequired')),

    dateOfBirth: Yup.string().required(t('register.validation.dateOfBirthRequired')),

    socialMediaUrl: Yup.string()
      .nullable()
      .test(
        'socialMediaUrlLength',
        t('register.validation.socialMediaUrlLength', { min: URL_MIN, max: URL_MAX }),
        (value) => {
          if (!value || value.trim() === '') {
            return true;
          }

          const trimmed = value.trim();
          return trimmed.length >= URL_MIN && trimmed.length <= URL_MAX;
        },
      ),

    biography: Yup.string()
      .nullable()
      .test(
        'biographyLength',
        t('register.validation.biographyLength', { min: BIO_MIN, max: BIO_MAX }),
        (value) => {
          if (!value || value.trim() === '') {
            return true;
          }

          const trimmed = value.trim();
          return trimmed.length >= BIO_MIN && trimmed.length <= BIO_MAX;
        },
      ),

    isPrivate: Yup.boolean(),

    image: Yup.mixed<File>()
      .nullable()
      .test('fileSize', t('register.validation.imageTooLarge'), (file) => {
        if (!file) {
          return true;
        }

        return file.size <= MAX_IMAGE_SIZE_BYTES;
      })
      .test(
        'fileType',
        t('register.validation.imageInvalidType', {
          types: ALLOWED_EXTENSIONS.join(', '),
        }),
        (file) => {
          if (!file) {
            return true;
          }

          const isAllowedType = ALLOWED_CONTENT_TYPES.includes(
            file.type as (typeof ALLOWED_CONTENT_TYPES)[number],
          );

          if (isAllowedType) {
            return true;
          }

          const lowerName = file.name.toLowerCase();
          const hasValidExtension = ALLOWED_EXTENSIONS.some((ext) => lowerName.endsWith(ext));

          return hasValidExtension;
        },
      ),
  });
