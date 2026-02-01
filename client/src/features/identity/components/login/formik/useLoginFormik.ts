import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { createLoginSchema } from '@/features/identity/components/login/validation/loginSchema';
import { useLogin } from '@/features/identity/hooks/useIdentity';
import type { LoginRequest } from '@/features/identity/types/identity';
import { routes } from '@/shared/lib/constants/api';
import { IsError } from '@/shared/lib/utils/utils';

const loginInitialValues: LoginRequest = {
  credentials: '',
  password: '',
  rememberMe: false,
};

export const useLoginFormik = () => {
  const navigate = useNavigate();
  const loginHandler = useLogin();
  const { t } = useTranslation('identity');

  const formik = useFormik<LoginRequest>({
    initialValues: loginInitialValues,
    validationSchema: createLoginSchema(t),
    onSubmit: async (values, { setErrors }) => {
      try {
        await loginHandler(values.credentials, values.password, values.rememberMe);
        navigate(routes.home);
      } catch (error) {
        const message = IsError(error) ? error.message : t('messages.unknownError');
        setErrors({ credentials: message });
      }
    },
  });

  return formik;
};


