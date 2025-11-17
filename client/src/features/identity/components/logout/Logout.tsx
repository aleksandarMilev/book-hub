import { type FC, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

const Logout: FC = () => {
  const { logout, username } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  useEffect(() => {
    const name = username;

    logout();
    showMessage(`Goodbye, ${name}.`, true);
    navigate(routes.home);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return null;
};

export default Logout;
