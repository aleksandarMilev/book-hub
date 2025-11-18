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
  url: 'Must be a valid URL',
};

export const articleSchema = Yup.object({
  title: Yup.string()
    .min(constraints.title.min, messages.min('Title', constraints.title.min))
    .max(constraints.title.max, messages.max('Title', constraints.title.max))
    .required(messages.required('Title')),
  introduction: Yup.string()
    .min(constraints.introduction.min, messages.min('Introduction', constraints.introduction.min))
    .max(constraints.introduction.max, messages.max('Introduction', constraints.introduction.max))
    .required(messages.required('Introduction')),
  imageUrl: Yup.string()
    .transform((v) => (v === '' ? null : v))
    .url(messages.url)
    .nullable()
    .notRequired(),
  content: Yup.string()
    .min(constraints.content.min, messages.min('Content', constraints.content.min))
    .max(constraints.content.max, messages.max('Content', constraints.content.max))
    .required(messages.required('Content')),
});
