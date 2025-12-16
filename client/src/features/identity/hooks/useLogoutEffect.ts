import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';
import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

export const useLogoutEffect = () => {
  const { logout, username } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { t } = useTranslation('identity');

  useEffect(() => {
    const name = username;

    logout();
    if (name) {
      showMessage(t('messages.goodbye', { username: name }), true);
    }

    navigate(routes.home);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
};
