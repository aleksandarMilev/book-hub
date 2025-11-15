import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { registerSchema } from '@/features/identity/components/register/validation/registerSchema.js';
import { useRegister } from '@/features/identity/hooks/useIdentity.js';
import type { RegisterFormValues } from '@/features/identity/types/identity.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils.js';

const registerInitialValues = {
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
};

export const useRegisterFormik = () => {
  const navigate = useNavigate();
  const registerHandler = useRegister();

  const formik = useFormik<RegisterFormValues>({
    initialValues: registerInitialValues,
    validationSchema: registerSchema,
    onSubmit: async (values, { setErrors }) => {
      try {
        await registerHandler(values.username, values.email, values.password);
        navigate(routes.home);
      } catch (error) {
        const message = IsError(error) ? error.message : 'An error occurred!';
        setErrors({ username: message });
      }
    },
  });

  return formik;
};
