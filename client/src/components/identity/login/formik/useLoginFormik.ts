import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { loginInitialValues, type LoginFormValues } from '../../../../api/identity/types/identity';
import { routes } from '../../../../common/constants/api';
import * as useIdentity from '../../../../hooks/useIdentity';

import { loginSchema } from '../validation/loginSchema';

export const useLoginFormik = () => {
  const navigate = useNavigate();
  const loginHandler = useIdentity.useLogin();

  const formik = useFormik<LoginFormValues>({
    initialValues: loginInitialValues,
    validationSchema: loginSchema,
    onSubmit: async (values, { setErrors }) => {
      try {
        await loginHandler(values.credentials, values.password, values.rememberMe);
        navigate(routes.home);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'An error occurred';
        setErrors({ credentials: message });
      }
    },
  });

  return formik;
};
