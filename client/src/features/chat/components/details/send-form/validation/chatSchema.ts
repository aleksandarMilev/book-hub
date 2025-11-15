import * as Yup from 'yup';

const lengths = {
  minMessage: 1,
  maxMessage: 5_000,
};

export const chatSchema = Yup.object({
  message: Yup.string()
    .min(lengths.minMessage, 'Message must contain at least 1 character')
    .max(lengths.maxMessage, 'Message must be less than 5000 characters')
    .required('Message is required!'),
});
