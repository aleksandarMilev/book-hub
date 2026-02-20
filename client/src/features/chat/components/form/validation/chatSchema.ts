import type { TFunction } from 'i18next';
import * as Yup from 'yup';

const constraints = {
  name: { min: 2, max: 200 },
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

export const chatSchema = (t: TFunction<'chats'>) =>
  Yup.object({
    name: Yup.string()
      .min(
        constraints.name.min,
        t('validation.min', {
          field: t('validation.fields.name'),
          min: constraints.name.min,
        }),
      )
      .max(
        constraints.name.max,
        t('validation.max', {
          field: t('validation.fields.name'),
          max: constraints.name.max,
        }),
      )
      .required(
        t('validation.required', {
          field: t('validation.fields.name'),
        }),
      ),

    image: Yup.mixed<File>()
      .nullable()
      .test(
        'fileSize',
        t('validation.image.tooLarge', { field: t('validation.fields.image') }),
        (file) => {
          if (!file) {
            return true;
          }

          return file.size <= MAX_IMAGE_SIZE_BYTES;
        },
      )
      .test(
        'fileType',
        t('validation.image.invalidType', {
          field: t('validation.fields.image'),
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

export type ChatFormValues = Yup.InferType<ReturnType<typeof chatSchema>>;

