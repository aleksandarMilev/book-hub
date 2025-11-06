import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import { useMessage } from '../../../../contexts/message/messageContext';
import * as hooks from '../../../../hooks/useProfile';
import { routes } from '../../../../common/constants/api';
import { profileSchema } from '../validation/profileSchema';
import type { Profile, ProfileInput } from '../../../../api/profile/types/profile';

export function useProfileFormik({
  profile = null,
  isEditMode = false,
}: {
  profile?: Profile | null;
  isEditMode?: boolean;
}) {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = hooks.useCreate();
  const editHandler = hooks.useEdit();

  const formik = useFormik<ProfileInput>({
    initialValues: {
      firstName: profile?.firstName || '',
      lastName: profile?.lastName || '',
      imageUrl: profile?.imageUrl || null,
      phoneNumber: profile?.phoneNumber || '',
      dateOfBirth: profile?.dateOfBirth || '',
      socialMediaUrl: profile?.socialMediaUrl || null,
      biography: profile?.biography || null,
      isPrivate: profile?.isPrivate ?? false,
    },
    validationSchema: profileSchema,
    onSubmit: async (values, { resetForm }) => {
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
      } catch (error: any) {
        showMessage(error?.message ?? 'Profile action failed.', false);
      }
    },
  });

  return formik;
}
