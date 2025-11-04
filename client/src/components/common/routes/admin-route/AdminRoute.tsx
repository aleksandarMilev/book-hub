import { useContext, type ReactElement } from 'react';
import { Navigate } from 'react-router-dom';

import { UserContext } from '../../../../contexts/user/userContext';
import type { Props } from '../types/props.type';
import { routes } from '../../../../common/constants/api';

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
