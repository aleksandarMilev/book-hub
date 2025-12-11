import * as Yup from 'yup';

const urlRangeMessage = 'URL length should be between 10 and 2000 characters long';

const lengths = {
  minName: 2,
  maxName: 100,
  minUrl: 10,
  maxUrl: 2_000,
  minBio: 10,
  maxBio: 1_000,
};

const MAX_IMAGE_SIZE_BYTES = 2 * 1_024 * 1_024;
const ALLOWED_EXTENSIONS = ['.jpg', '.jpeg', '.png', '.webp', '.avif'] as const;
const ALLOWED_CONTENT_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/avif'] as const;

export const profileSchema = Yup.object({
  firstName: Yup.string()
    .min(
      lengths.minName,
      `First name length should be between ${lengths.minName} and ${lengths.maxName} characters long`,
    )
    .max(
      lengths.maxName,
      `First name length should be between ${lengths.minName} and ${lengths.maxName} characters long`,
    )
    .required('First name is required'),

  lastName: Yup.string()
    .min(
      lengths.minName,
      `Last name length should be between ${lengths.minName} and ${lengths.maxName} characters long`,
    )
    .max(
      lengths.maxName,
      `Last name length should be between ${lengths.minName} and ${lengths.maxName} characters long`,
    )
    .required('Last name is required'),

  dateOfBirth: Yup.string().required('Date of birth is required'),

  socialMediaUrl: Yup.string()
    .transform((v) => (v === '' ? null : v))
    .url('Invalid URL')
    .min(lengths.minUrl, urlRangeMessage)
    .max(lengths.maxUrl, urlRangeMessage)
    .nullable(),

  biography: Yup.string()
    .transform((v) => (v === '' ? null : v))
    .min(
      lengths.minBio,
      `Biography length should be between ${lengths.minBio} and ${lengths.maxBio} characters long`,
    )
    .max(
      lengths.maxBio,
      `Biography length should be between ${lengths.minBio} and ${lengths.maxBio} characters long`,
    )
    .nullable(),

  isPrivate: Yup.boolean().required(),

  image: Yup.mixed<File>()
    .nullable()
    .test('fileSize', 'Image is too large (max 2MB)', (file) => {
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
