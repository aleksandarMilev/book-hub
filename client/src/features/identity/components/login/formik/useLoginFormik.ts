import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { loginSchema } from '@/features/identity/components/login/validation/loginSchema.js';
import { useLogin } from '@/features/identity/hooks/useIdentity.js';
import type { LoginRequest } from '@/features/identity/types/identity.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils/utils.js';

const loginInitialValues = {
  credentials: '',
  password: '',
  rememberMe: false,
};

export const useLoginFormik = () => {
  const navigate = useNavigate();
  const loginHandler = useLogin();

  const formik = useFormik<LoginRequest>({
    initialValues: loginInitialValues,
    validationSchema: loginSchema,
    onSubmit: async (values, { setErrors }) => {
      try {
        await loginHandler(values.credentials, values.password, values.rememberMe);
        navigate(routes.home);
      } catch (error) {
        const message = IsError(error) ? error.message : 'An error occurred!';
        setErrors({ credentials: message });
      }
    },
  });

  return formik;
};
