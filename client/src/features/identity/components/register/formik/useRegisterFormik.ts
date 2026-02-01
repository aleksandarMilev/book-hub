import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { createRegisterSchema } from '@/features/identity/components/register/validation/registerSchema';
import { useRegister } from '@/features/identity/hooks/useIdentity';
import type { RegisterFormValues } from '@/features/identity/types/identity';
import { routes } from '@/shared/lib/constants/api';
import { IsError } from '@/shared/lib/utils/utils';

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
    initialStatus: { submitError: '' as string },
    onSubmit: async (values, { setStatus, setSubmitting }) => {
      setStatus({ submitError: '' });

      try {
        await registerHandler(values);
        navigate(routes.home);
      } catch (error) {
        const errorMessage = IsError(error) ? error.message : t('messages.unknownError');

        setStatus({ submitError: errorMessage });
        setSubmitting(false);
      }
    },
  });

  return formik;
};


