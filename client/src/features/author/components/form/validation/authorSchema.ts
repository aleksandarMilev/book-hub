import * as Yup from 'yup';

import i18n from '@/shared/i18n/i18n.js';

const constraints = {
  name: { min: 2, max: 200 },
  penName: { min: 2, max: 200 },
  biography: { min: 50, max: 10_000 },
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

const messages = {
  required: (field: string) => `${field} is required!`,
  min: (field: string, min: number) => `${field} must be at least ${min} characters long`,
  max: (field: string, max: number) => `${field} must be less than ${max} characters`,
  datePast: (field: string) => `${field} must be in the past`,
  deathBeforeBirth: 'Date of death cannot be earlier than date of birth',
};

export const authorSchema = Yup.object({
  name: Yup.string()
    .min(constraints.name.min, messages.min('Name', constraints.name.min))
    .max(constraints.name.max, messages.max('Name', constraints.name.max))
    .required(messages.required('Name')),
  penName: Yup.string()
    .min(constraints.penName.min, messages.min('Pen Name', constraints.penName.min))
    .max(constraints.penName.max, messages.max('Pen Name', constraints.penName.max))
    .nullable(),
  image: Yup.mixed<File>()
    .nullable()
    .test(
      'fileSize',
      () => i18n.t('authors:validation.image.tooLarge') as string,
      (file) => {
        if (!file) {
          return true;
        }

        return file.size <= MAX_IMAGE_SIZE_BYTES;
      },
    )
    .test(
      'fileType',
      () =>
        i18n.t('authors:validation.image.invalidType', {
          types: ALLOWED_EXTENSIONS.join(', '),
        }) as string,
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
  bornAt: Yup.date()
    .transform((value, originalValue) => (originalValue === '' ? null : value))
    .max(new Date(), messages.datePast('Date of birth'))
    .nullable(),
  diedAt: Yup.date()
    .max(new Date(), messages.datePast('Date of death'))
    .min(Yup.ref('bornAt'), messages.deathBeforeBirth)
    .nullable(),
  gender: Yup.string().required(messages.required('Gender')),
  nationality: Yup.number().nullable(),
  biography: Yup.string()
    .min(constraints.biography.min, messages.min('Biography', constraints.biography.min))
    .max(constraints.biography.max, messages.max('Biography', constraints.biography.max))
    .required(messages.required('Biography')),
});

export type AuthorFormValues = Yup.InferType<typeof authorSchema>;
