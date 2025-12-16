import { useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';

import { useRegisterFormik } from '../components/register/formik/useRegisterFormik.js';

export const useRegisterPage = () => {
  const formik = useRegisterFormik();
  const { t } = useTranslation('identity');

  const isSubmitting = formik.isSubmitting;
  const submitErrorRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (formik.status?.submitError) {
      submitErrorRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
      submitErrorRef.current?.focus();
    }
  }, [formik.status?.submitError]);

  return { t, isSubmitting, formik, submitErrorRef };
};
