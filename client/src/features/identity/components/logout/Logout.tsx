import { type FC, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

const Logout: FC = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    logout();
    navigate(routes.home);
  }, [logout, navigate]);

  return null;
};

export default Logout;
