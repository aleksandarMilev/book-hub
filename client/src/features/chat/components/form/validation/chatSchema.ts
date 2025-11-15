import * as Yup from 'yup';

const lengths = {
  minName: 2,
  maxName: 200,
};

export const chatSchema = Yup.object({
  name: Yup.string().min(lengths.minName).max(lengths.maxName).required('Name is required!'),
  imageUrl: Yup.string()
    .transform((v) => (v === '' ? null : v))
    .url('Invalid URL')
    .min(10)
    .max(2000)
    .nullable(),
});
