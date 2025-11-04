import * as Yup from 'yup';

const constraints = {
  name: { min: 2, max: 200 },
  penName: { min: 2, max: 200 },
  imageUrl: { min: 10, max: 2_000 },
  biography: { min: 50, max: 10_000 },
};

const messages = {
  required: (field: string) => `${field} is required!`,
  min: (field: string, min: number) => `${field} must be at least ${min} characters long`,
  max: (field: string, max: number) => `${field} must be less than ${max} characters`,
  url: 'Must be a valid URL',
  datePast: (field: string) => `${field} must be in the past`,
  deathBeforeBirth: 'Date of death cannot be earlier than date of birth',
};

export const authorSchema = Yup.object({
  name: Yup.string()
    .min(constraints.name.min, messages.min('Name', constraints.name.min))
    .max(constraints.name.max, messages.max('Name', constraints.name.max))
    .required(messages.required('Name')),
  penName: Yup.string()
    .min(constraints.penName.min, messages.min('Pen Name', constraints.penName.min))
    .max(constraints.penName.max, messages.max('Pen Name', constraints.penName.max))
    .nullable(),
  imageUrl: Yup.string()
    .url(messages.url)
    .min(constraints.imageUrl.min, messages.min('Image URL', constraints.imageUrl.min))
    .max(constraints.imageUrl.max, messages.max('Image URL', constraints.imageUrl.max))
    .nullable(),
  bornAt: Yup.date()
    .transform((value, originalValue) => (originalValue === '' ? null : value))
    .max(new Date(), messages.datePast('Date of birth'))
    .nullable(),
  diedAt: Yup.date()
    .max(new Date(), messages.datePast('Date of death'))
    .min(Yup.ref('bornAt'), messages.deathBeforeBirth)
    .nullable(),
  gender: Yup.string().required(messages.required('Gender')),
  nationality: Yup.string().nullable(),
  biography: Yup.string()
    .min(constraints.biography.min, messages.min('Biography', constraints.biography.min))
    .max(constraints.biography.max, messages.max('Biography', constraints.biography.max))
    .required(messages.required('Biography')),
});
