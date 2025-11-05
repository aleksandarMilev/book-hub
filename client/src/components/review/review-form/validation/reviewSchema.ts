import * as Yup from 'yup';

const constraints = {
  content: { min: 10, max: 5_000 },
  rating: { min: 1, max: 5 },
};

const messages = {
  required: (field: string) => `${field} is required!`,
  min: (field: string, min: number) => `${field} must be at least ${min} characters`,
  max: (field: string, max: number) => `${field} cannot exceed ${max} characters`,
};

export const reviewSchema = Yup.object({
  content: Yup.string()
    .min(constraints.content.min, messages.min('Review', constraints.content.min))
    .max(constraints.content.max, messages.max('Review', constraints.content.max))
    .required(messages.required('Review content')),
  rating: Yup.number()
    .min(constraints.rating.min, 'Please select a rating')
    .max(constraints.rating.max, 'Rating cannot exceed 5')
    .required('Please select a rating'),
});
