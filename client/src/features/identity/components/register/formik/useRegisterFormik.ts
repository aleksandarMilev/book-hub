import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { registerSchema } from '@/features/identity/components/register/validation/registerSchema';
import * as hooks from '@/features/identity/hooks/useIdentity';
import type { RegisterFormValues } from '@/features/identity/types/identity';
import { routes } from '@/shared/lib/constants/api';
import { IsError } from '@/shared/lib/utils';

const registerInitialValues = {
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
};

export const useRegisterFormik = () => {
  const navigate = useNavigate();
  const registerHandler = hooks.useRegister();

  const formik = useFormik<RegisterFormValues>({
    initialValues: registerInitialValues,
    validationSchema: registerSchema,
    onSubmit: async (values, { setErrors }) => {
      try {
        await registerHandler(values);
        navigate(routes.home);
      } catch (error) {
        const message = IsError(error) ? error.message : 'An error occurred';
        setErrors({ username: message });
      }
    },
  });

  return formik;
};
