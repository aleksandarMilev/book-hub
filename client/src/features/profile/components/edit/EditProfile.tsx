import type { FC } from 'react';

import ProfileForm from '@/features/profile/components/form/ProfileForm';
import { useMineProfile } from '@/features/profile/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';

const EditProfile: FC = () => {
  const { profile, isFetching } = useMineProfile();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return <ProfileForm profile={profile} />;
};

export default EditProfile;


