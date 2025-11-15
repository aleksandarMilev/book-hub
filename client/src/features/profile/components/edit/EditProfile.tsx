import type { FC } from 'react';

import ProfileForm from '@/features/profile/components/form/ProfileForm.js';
import { useMineProfile } from '@/features/profile/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';

const EditProfile: FC = () => {
  const { profile, isFetching } = useMineProfile();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return <ProfileForm profile={profile} isEditMode />;
};

export default EditProfile;
