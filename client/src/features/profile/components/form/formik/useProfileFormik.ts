import { useFormik } from 'formik';
import { useMemo } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { createProfileSchema } from '@/features/profile/components/form/validation/profileSchema';
import { useEdit } from '@/features/profile/hooks/useCrud';
import type { CreateProfile, Profile } from '@/features/profile/types/profile';
import { routes } from '@/shared/lib/constants/api';
import { IsError } from '@/shared/lib/utils/utils';
import { useMessage } from '@/shared/stores/message/message';

type Props = { profile?: Profile | null };

const normalizeDate = (date?: string | null): string => {
  if (!date) {
    return '';
  }

  return date.split('T')[0]!;
};

export const useProfileFormik = ({ profile = null }: Props) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const editHandler = useEdit();
  const { t } = useTranslation('profiles');

  const initialValues = useMemo(
    () => ({
      firstName: profile?.firstName ?? '',
      lastName: profile?.lastName ?? '',
      dateOfBirth: normalizeDate(profile?.dateOfBirth ?? null),
      socialMediaUrl: profile?.socialMediaUrl ?? null,
      biography: profile?.biography ?? null,
      isPrivate: profile?.isPrivate ?? false,
      image: null,
      removeImage: false,
    }),
    [profile],
  );

  const formik = useFormik<CreateProfile>({
    initialValues,
    enableReinitialize: true,
    validationSchema: createProfileSchema(t),
    onSubmit: async (values, { resetForm, setSubmitting }) => {
      try {
        await editHandler(values);
        showMessage(t('messages.editSuccess'), true);

        navigate(routes.profile, { replace: true });
      } catch (error) {
        const message = IsError(error) ? error.message : t('messages.editFailed');
        showMessage(message, false);
      } finally {
        setSubmitting(false);
        resetForm();
      }
    },
  });

  return formik;
};


