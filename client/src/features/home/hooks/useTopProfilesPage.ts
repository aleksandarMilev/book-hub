import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { useTopProfiles } from '@/features/profile/hooks/useCrud';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

export const useTopProfilesPage = () => {
  const { t } = useTranslation('home');
  const { userId } = useAuth();
  const navigate = useNavigate();
  const { profiles, isFetching, error } = useTopProfiles();

  const onProfileClickHandler = (profileId: string): void => {
    navigate(routes.profile, {
      state: { id: profileId === userId ? null : profileId },
    });
  };

  return { t, profiles, isFetching, error, onProfileClickHandler };
};


