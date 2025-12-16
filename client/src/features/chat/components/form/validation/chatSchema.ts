import * as Yup from 'yup';

const constraints = {
  name: { min: 2, max: 200 },
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

const FIELD_LABELS = {
  name: 'Name',
  image: 'Image',
} as const;

export const chatSchema = Yup.object({
  name: Yup.string()
    .min(
      constraints.name.min,
      `${FIELD_LABELS.name} must be at least ${constraints.name.min} characters`,
    )
    .max(
      constraints.name.max,
      `${FIELD_LABELS.name} must be at most ${constraints.name.max} characters`,
    )
    .required(`${FIELD_LABELS.name} is required!`),

  image: Yup.mixed<File>()
    .nullable()
    .test('fileSize', `${FIELD_LABELS.image} is too large (max 2MB)`, (file) => {
      if (!file) {
        return true;
      }

      return file.size <= MAX_IMAGE_SIZE_BYTES;
    })
    .test(
      'fileType',
      `${FIELD_LABELS.image} has an invalid type. Allowed: ${ALLOWED_EXTENSIONS.join(', ')}`,
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

export type ChatFormValues = Yup.InferType<typeof chatSchema>;
