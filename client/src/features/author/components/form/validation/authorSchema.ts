import { isValid, parseISO } from 'date-fns';
import * as Yup from 'yup';

import i18n from '@/shared/i18n/i18n';

const constraints = {
  name: { min: 2, max: 200 },
  penName: { min: 2, max: 200 },
  biography: { min: 50, max: 10_000 },
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

const FIELD_KEYS = {
  name: 'authors:validation.fields.name',
  penName: 'authors:validation.fields.penName',
  biography: 'authors:validation.fields.biography',
  bornAt: 'authors:validation.fields.bornAt',
  diedAt: 'authors:validation.fields.diedAt',
  gender: 'authors:validation.fields.gender',
  image: 'authors:validation.fields.image',
} as const;

const isIsoDate = (value: string) => isValid(parseISO(value));
const getFieldLabel = (field: keyof typeof FIELD_KEYS) => i18n.t(FIELD_KEYS[field]);

export const authorSchema = Yup.object({
  name: Yup.string()
    .min(constraints.name.min, () =>
      i18n.t('authors:validation.min', {
        field: getFieldLabel('name'),
        min: constraints.name.min,
      }),
    )
    .max(constraints.name.max, () =>
      i18n.t('authors:validation.max', {
        field: getFieldLabel('name'),
        max: constraints.name.max,
      }),
    )
    .required(() =>
      i18n.t('authors:validation.required', {
        field: getFieldLabel('name'),
      }),
    ),
  penName: Yup.string()
    .min(constraints.penName.min, () =>
      i18n.t('authors:validation.min', {
        field: getFieldLabel('penName'),
        min: constraints.penName.min,
      }),
    )
    .max(constraints.penName.max, () =>
      i18n.t('authors:validation.max', {
        field: getFieldLabel('penName'),
        max: constraints.penName.max,
      }),
    )
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
  bornAt: Yup.string()
    .transform((_, originalValue) => (originalValue === '' ? null : originalValue))
    .nullable()
    .test(
      'bornAt-valid',
      () =>
        i18n.t('authors:validation.invalidDate', {
          field: getFieldLabel('bornAt'),
        }) as string,
      (value) => !value || isIsoDate(value),
    )
    .test(
      'bornAt-past',
      () =>
        i18n.t('authors:validation.datePast', {
          field: getFieldLabel('bornAt'),
        }) as string,
      (value) => {
        if (!value) {
          return true;
        }

        return parseISO(value) <= new Date();
      },
    ),
  diedAt: Yup.string()
    .transform((_, originalValue) => (originalValue === '' ? null : originalValue))
    .nullable()
    .test(
      'diedAt-valid',
      () =>
        i18n.t('authors:validation.invalidDate', {
          field: getFieldLabel('diedAt'),
        }) as string,
      (value) => !value || isIsoDate(value),
    )
    .test(
      'diedAt-past',
      () =>
        i18n.t('authors:validation.datePast', {
          field: getFieldLabel('diedAt'),
        }) as string,
      (value) => {
        if (!value) {
          return true;
        }

        return parseISO(value) <= new Date();
      },
    )
    .test(
      'deathAfterBirth',
      () => i18n.t('authors:validation.deathBeforeBirth') as string,
      function (diedAt) {
        const { bornAt } = this.parent as { bornAt: string | null };
        if (!diedAt || !bornAt) {
          return true;
        }

        return parseISO(diedAt) >= parseISO(bornAt);
      },
    ),

  gender: Yup.string().required(() =>
    i18n.t('authors:validation.required', {
      field: getFieldLabel('gender'),
    }),
  ),
  nationality: Yup.number().nullable(),
  biography: Yup.string()
    .min(constraints.biography.min, () =>
      i18n.t('authors:validation.min', {
        field: getFieldLabel('biography'),
        min: constraints.biography.min,
      }),
    )
    .max(constraints.biography.max, () =>
      i18n.t('authors:validation.max', {
        field: getFieldLabel('biography'),
        max: constraints.biography.max,
      }),
    )
    .required(() =>
      i18n.t('authors:validation.required', {
        field: getFieldLabel('biography'),
      }),
    ),
});

export type AuthorFormValues = Yup.InferType<typeof authorSchema>;


