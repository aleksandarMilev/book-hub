import type { FC } from 'react';
import * as hooks from '../../../hooks/useProfile';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import ProfileForm from '../profile-form/ProfileForm';

const EditProfile: FC = () => {
  const { profile, isFetching } = hooks.useMineProfile();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return <ProfileForm profile={profile} isEditMode />;
};

export default EditProfile;
