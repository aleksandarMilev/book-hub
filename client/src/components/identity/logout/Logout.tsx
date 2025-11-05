import { useContext, useEffect, type FC } from 'react';
import { useNavigate } from 'react-router-dom';

import { routes } from '../../../common/constants/api';
import { UserContext } from '../../../contexts/user/userContext';

const Logout: FC = () => {
  const navigate = useNavigate();
  const { logout } = useContext(UserContext);

  useEffect(() => {
    logout();
    navigate(routes.home);
  }, [logout, navigate]);

  return null;
};

export default Logout;
