import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { loginSchema } from '@/features/identity/components/login/validation/loginSchema';
import * as hooks from '@/features/identity/hooks/useIdentity';
import type { LoginRequest } from '@/features/identity/types/identity';
import { routes } from '@/shared/lib/constants/api';

const loginInitialValues = {
  credentials: '',
  password: '',
  rememberMe: false,
};

export const useLoginFormik = () => {
  const navigate = useNavigate();
  const loginHandler = hooks.useLogin();

  const formik = useFormik<LoginRequest>({
    initialValues: loginInitialValues,
    validationSchema: loginSchema,
    onSubmit: async (values, { setErrors }) => {
      try {
        await loginHandler(values);
        navigate(routes.home);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'An error occurred';
        setErrors({ credentials: message });
      }
    },
  });

  return formik;
};
