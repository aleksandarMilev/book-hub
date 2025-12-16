import { useTranslation } from 'react-i18next';
import { useLoginFormik } from '../components/login/formik/useLoginFormik.js';

export const useLoginPage = () => {
  const formik = useLoginFormik();
  const { t } = useTranslation('identity');

  return { t, formik };
};
