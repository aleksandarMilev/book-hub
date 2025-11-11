import * as Yup from 'yup';

const urlRangeMessage = 'URL length should be between 10 and 2000 characters long';

const lengths = {
  minName: 2,
  maxName: 100,
  minUrl: 10,
  maxUrl: 2_000,
  minPhone: 8,
  maxPhone: 15,
  minBio: 10,
  maxBio: 1_000,
};

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
  imageUrl: Yup.string()
    .transform((v) => (v === '' ? null : v))
    .url('Invalid URL')
    .min(lengths.minUrl, urlRangeMessage)
    .max(lengths.maxUrl, urlRangeMessage)
    .required('Image URL is required')
    .nullable(),
  phoneNumber: Yup.string()
    .matches(
      /^(?:\+359|0)\d{8,14}$/,
      'Phone number must start with +359 or 0 and be followed by 8 to 14 digits',
    )
    .min(
      lengths.minPhone,
      `Phone Number length should be between ${lengths.minPhone} and ${lengths.maxPhone} characters long`,
    )
    .max(
      lengths.maxPhone,
      `Phone Number length should be between ${lengths.minPhone} and ${lengths.maxPhone} characters long`,
    )
    .required('Phone Number is required'),
  dateOfBirth: Yup.date().required('Date of birth is required').typeError('Invalid date'),
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
});
