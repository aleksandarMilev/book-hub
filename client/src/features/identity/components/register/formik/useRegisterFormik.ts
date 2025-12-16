import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { createRegisterSchema } from '@/features/identity/components/register/validation/registerSchema.js';
import { useRegister } from '@/features/identity/hooks/useIdentity.js';
import type { RegisterFormValues } from '@/features/identity/types/identity.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils/utils.js';

const registerInitialValues: RegisterFormValues = {
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
  firstName: '',
  lastName: '',
  dateOfBirth: '',
  socialMediaUrl: '',
  biography: '',
  isPrivate: false,
  image: null,
};

export const useRegisterFormik = () => {
  const navigate = useNavigate();
  const registerHandler = useRegister();
  const { t } = useTranslation('identity');

  const formik = useFormik<RegisterFormValues>({
    initialValues: registerInitialValues,
    validationSchema: createRegisterSchema(t),
    onSubmit: async (values, { setErrors }) => {
      try {
        await registerHandler(values);
        navigate(routes.home);
      } catch (error) {
        const errorMessage = IsError(error) ? error.message : t('messages.unknownError');
        setErrors({ username: errorMessage });
      }
    },
  });

  return formik;
};
