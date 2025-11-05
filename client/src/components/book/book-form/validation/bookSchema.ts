import * as Yup from 'yup';

const constraints = {
  title: { min: 2, max: 200 },
  imageUrl: { min: 10, max: 2_000 },
  shortDescription: { min: 10, max: 200 },
  longDescription: { min: 100, max: 10_000 },
};

const messages = {
  required: (field: string) => `${field} is required!`,
  min: (field: string, min: number) => `${field} must be at least ${min} characters long`,
  max: (field: string, max: number) => `${field} must be less than ${max} characters`,
  url: 'Must be a valid URL',
  datePast: (field: string) => `${field} must be in the past`,
};

export const bookSchema = Yup.object({
  title: Yup.string()
    .min(constraints.title.min, messages.min('Title', constraints.title.min))
    .max(constraints.title.max, messages.max('Title', constraints.title.max))
    .required(messages.required('Title')),
  authorId: Yup.number().nullable(),
  imageUrl: Yup.string()
    .url(messages.url)
    .min(constraints.imageUrl.min, messages.min('Image URL', constraints.imageUrl.min))
    .max(constraints.imageUrl.max, messages.max('Image URL', constraints.imageUrl.max))
    .nullable(),
  shortDescription: Yup.string()
    .min(
      constraints.shortDescription.min,
      messages.min('Short Description', constraints.shortDescription.min),
    )
    .max(
      constraints.shortDescription.max,
      messages.max('Short Description', constraints.shortDescription.max),
    )
    .required(messages.required('Short Description')),
  longDescription: Yup.string()
    .min(
      constraints.longDescription.min,
      messages.min('Long Description', constraints.longDescription.min),
    )
    .max(
      constraints.longDescription.max,
      messages.max('Long Description', constraints.longDescription.max),
    )
    .required(messages.required('Long Description')),
  genres: Yup.array()
    .of(Yup.number().required())
    .min(1, 'At least one genre is required')
    .required('Genres are required'),
  publishedDate: Yup.date()
    .transform((value, original) => (original === '' ? null : value))
    .max(new Date(), messages.datePast('Published Date'))
    .nullable(),
});
