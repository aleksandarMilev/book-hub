import { useContext } from 'react';
import { Navigate } from 'react-router-dom';

import { routes } from '../../../../common/constants/api';
import { UserContext } from '../../../../contexts/user/userContext';

import type { Props } from '../types/props.type';

export default function AuthenticatedRoute({ element }: Props) {
  const { isAuthenticated, isAdmin } = useContext(UserContext);

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!isAdmin) {
    return <Navigate to={routes.accessDenied} replace />;
  }

  return element;
}
