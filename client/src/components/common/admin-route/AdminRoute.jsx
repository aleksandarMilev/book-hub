import { useContext } from 'react';
import { Navigate } from 'react-router-dom';

import { UserContext } from '../../../contexts/userContext';
import { routes } from '../../../common/constants/api';

export default function AuthenticatedRoute({ element }) {
  const { isAuthenticated, isAdmin } = useContext(UserContext);

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!isAdmin) {
    return <Navigate to={routes.accessDenied} replace />;
  }

  return element;
}
