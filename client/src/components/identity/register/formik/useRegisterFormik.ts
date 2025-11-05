import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as useIdentity from '../../../../hooks/useIdentity';
import { routes } from '../../../../common/constants/api';
import { registerSchema } from '../validation/registerSchema';
import {
  registerInitialValues,
  type RegisterFormValues,
} from '../../../../api/identity/types/identity';

export const useRegisterFormik = () => {
  const navigate = useNavigate();
  const registerHandler = useIdentity.useRegister();

  const formik = useFormik<RegisterFormValues>({
    initialValues: registerInitialValues,
    validationSchema: registerSchema,
    onSubmit: async (values, { setErrors }) => {
      try {
        await registerHandler(values.username, values.email, values.password);
        navigate(routes.home);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'An error occurred';
        setErrors({ username: message });
      }
    },
  });

  return formik;
};
