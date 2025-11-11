import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { profileSchema } from '@/features/profile/components/form/validation/profileSchema';
import { useCreate, useEdit } from '@/features/profile/hooks/useCrud';
import type { CreateProfile, Profile } from '@/features/profile/types/profile';
import { routes } from '@/shared/lib/constants/api';
import { IsError } from '@/shared/lib/utils';
import { useMessage } from '@/shared/stores/message/message';

export const useProfileFormik = ({
  profile = null,
  isEditMode = false,
}: {
  profile?: Profile | null;
  isEditMode?: boolean;
}) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const formik = useFormik<CreateProfile>({
    initialValues: {
      firstName: profile?.firstName || '',
      lastName: profile?.lastName || '',
      imageUrl: profile?.imageUrl || '',
      phoneNumber: profile?.phoneNumber || '',
      dateOfBirth: profile?.dateOfBirth || '',
      socialMediaUrl: profile?.socialMediaUrl ?? null,
      biography: profile?.biography ?? null,
      isPrivate: profile?.isPrivate ?? false,
    },
    validationSchema: profileSchema,
    onSubmit: async (values, { resetForm, setSubmitting }) => {
      try {
        if (isEditMode) {
          await editHandler(values);
          showMessage('You have successfully edited your profile!', true);
        } else {
          await createHandler(values);

          showMessage('You have successfully created your profile!', true);
          resetForm();
        }

        navigate(routes.profile, { replace: true });
      } catch (error) {
        const message = IsError(error) ? error.message : 'Profile action failed.';
        showMessage(message, false);
      } finally {
        setSubmitting(false);
      }
    },
  });

  return formik;
};
