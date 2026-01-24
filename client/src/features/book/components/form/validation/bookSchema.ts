import { isValid, parseISO } from 'date-fns';
import * as Yup from 'yup';

import i18n from '@/shared/i18n/i18n.js';

const constraints = {
  title: { min: 2, max: 200 },
  shortDescription: { min: 10, max: 200 },
  longDescription: { min: 100, max: 10_000 },
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

const FIELD_KEYS = {
  title: 'books:validation.fields.title',
  shortDescription: 'books:validation.fields.shortDescription',
  longDescription: 'books:validation.fields.longDescription',
  publishedDate: 'books:validation.fields.publishedDate',
  image: 'books:validation.fields.image',
  genres: 'books:validation.fields.genres',
} as const;

const isIsoDate = (value: string) => isValid(parseISO(value));
const getFieldLabel = (field: keyof typeof FIELD_KEYS) => i18n.t(FIELD_KEYS[field]);

export const bookSchema = Yup.object({
  title: Yup.string()
    .min(constraints.title.min, () =>
      i18n.t('books:validation.min', {
        field: getFieldLabel('title'),
        min: constraints.title.min,
      }),
    )
    .max(constraints.title.max, () =>
      i18n.t('books:validation.max', {
        field: getFieldLabel('title'),
        max: constraints.title.max,
      }),
    )
    .required(() =>
      i18n.t('books:validation.required', {
        field: getFieldLabel('title'),
      }),
    ),
  authorId: Yup.string().nullable(),
  image: Yup.mixed<File>()
    .nullable()
    .test(
      'fileSize',
      () => i18n.t('books:validation.image.tooLarge') as string,
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
        i18n.t('books:validation.image.invalidType', {
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
  shortDescription: Yup.string()
    .min(constraints.shortDescription.min, () =>
      i18n.t('books:validation.min', {
        field: getFieldLabel('shortDescription'),
        min: constraints.shortDescription.min,
      }),
    )
    .max(constraints.shortDescription.max, () =>
      i18n.t('books:validation.max', {
        field: getFieldLabel('shortDescription'),
        max: constraints.shortDescription.max,
      }),
    )
    .required(() =>
      i18n.t('books:validation.required', {
        field: getFieldLabel('shortDescription'),
      }),
    ),
  longDescription: Yup.string()
    .min(constraints.longDescription.min, () =>
      i18n.t('books:validation.min', {
        field: getFieldLabel('longDescription'),
        min: constraints.longDescription.min,
      }),
    )
    .max(constraints.longDescription.max, () =>
      i18n.t('books:validation.max', {
        field: getFieldLabel('longDescription'),
        max: constraints.longDescription.max,
      }),
    )
    .required(() =>
      i18n.t('books:validation.required', {
        field: getFieldLabel('longDescription'),
      }),
    ),
  genres: Yup.array()
    .of(Yup.string().required())
    .min(1, () =>
      i18n.t('books:validation.genres.min', {
        field: getFieldLabel('genres'),
      }),
    )
    .required(() =>
      i18n.t('books:validation.required', {
        field: getFieldLabel('genres'),
      }),
    ),
  publishedDate: Yup.string()
    .transform((_, originalValue) => (originalValue === '' ? null : originalValue))
    .nullable()
    .test(
      'publishedDate-valid',
      () =>
        i18n.t('books:validation.invalidDate', {
          field: getFieldLabel('publishedDate'),
        }) as string,
      (value) => !value || isIsoDate(value),
    ),
});

export type BookFormValues = Yup.InferType<typeof bookSchema>;
