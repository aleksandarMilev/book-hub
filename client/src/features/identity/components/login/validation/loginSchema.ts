import * as Yup from 'yup';

export const loginSchema = Yup.object({
  credentials: Yup.string().required('Username or Email is required'),
  password: Yup.string().required('Password is required'),
  rememberMe: Yup.boolean(),
});
