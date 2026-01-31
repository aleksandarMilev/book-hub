import * as Yup from 'yup';

import i18n from '@/shared/i18n/i18n';

const constraints = {
  title: { min: 10, max: 100 },
  introduction: { min: 10, max: 500 },
  content: { min: 100, max: 50_000 },
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

const FIELD_KEYS = {
  title: 'articles:validation.fields.title',
  introduction: 'articles:validation.fields.introduction',
  content: 'articles:validation.fields.content',
  image: 'articles:validation.fields.image',
} as const;

const getFieldLabel = (field: keyof typeof FIELD_KEYS) => i18n.t(FIELD_KEYS[field]);

export const articleSchema = Yup.object({
  title: Yup.string()
    .min(constraints.title.min, () =>
      i18n.t('articles:validation.min', {
        field: getFieldLabel('title'),
        min: constraints.title.min,
      }),
    )
    .max(constraints.title.max, () =>
      i18n.t('articles:validation.max', {
        field: getFieldLabel('title'),
        max: constraints.title.max,
      }),
    )
    .required(() =>
      i18n.t('articles:validation.required', {
        field: getFieldLabel('title'),
      }),
    ),
  introduction: Yup.string()
    .min(constraints.introduction.min, () =>
      i18n.t('articles:validation.min', {
        field: getFieldLabel('introduction'),
        min: constraints.introduction.min,
      }),
    )
    .max(constraints.introduction.max, () =>
      i18n.t('articles:validation.max', {
        field: getFieldLabel('introduction'),
        max: constraints.introduction.max,
      }),
    )
    .required(() =>
      i18n.t('articles:validation.required', {
        field: getFieldLabel('introduction'),
      }),
    ),
  content: Yup.string()
    .min(constraints.content.min, () =>
      i18n.t('articles:validation.min', {
        field: getFieldLabel('content'),
        min: constraints.content.min,
      }),
    )
    .max(constraints.content.max, () =>
      i18n.t('articles:validation.max', {
        field: getFieldLabel('content'),
        max: constraints.content.max,
      }),
    )
    .required(() =>
      i18n.t('articles:validation.required', {
        field: getFieldLabel('content'),
      }),
    ),
  image: Yup.mixed<File>()
    .nullable()
    .test(
      'fileSize',
      () => i18n.t('articles:validation.image.tooLarge') as string,
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
        i18n.t('articles:validation.image.invalidType', {
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
});

export type ArticleFormValues = Yup.InferType<typeof articleSchema>;


