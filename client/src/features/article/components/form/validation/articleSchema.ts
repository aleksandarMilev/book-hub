import * as Yup from 'yup';

const constraints = {
  title: { min: 10, max: 100 },
  introduction: { min: 10, max: 500 },
  content: { min: 100, max: 50_000 },
};

const messages = {
  required: (field: string) => `${field} is required`,
  min: (field: string, min: number) => `${field} must be at least ${min} characters long`,
  max: (field: string, max: number) => `${field} must be less than ${max} characters`,
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

export const articleSchema = Yup.object({
  title: Yup.string()
    .min(constraints.title.min, messages.min('Title', constraints.title.min))
    .max(constraints.title.max, messages.max('Title', constraints.title.max))
    .required(messages.required('Title')),
  introduction: Yup.string()
    .min(constraints.introduction.min, messages.min('Introduction', constraints.introduction.min))
    .max(constraints.introduction.max, messages.max('Introduction', constraints.introduction.max))
    .required(messages.required('Introduction')),
  content: Yup.string()
    .min(constraints.content.min, messages.min('Content', constraints.content.min))
    .max(constraints.content.max, messages.max('Content', constraints.content.max))
    .required(messages.required('Content')),
  image: Yup.mixed<File>()
    .nullable()
    .test('fileSize', 'Image must be smaller than 2 MB.', (file) => {
      if (!file) {
        return true;
      }

      return file.size <= MAX_IMAGE_SIZE_BYTES;
    })
    .test('fileType', `Invalid image type. Allowed: ${ALLOWED_EXTENSIONS.join(', ')}`, (file) => {
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
    }),
});

export type ArticleFormValues = Yup.InferType<typeof articleSchema>;
