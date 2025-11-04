import { useContext } from 'react';
import { Navigate } from 'react-router-dom';

import { routes } from '../../../../common/constants/api';
import type { Props } from '../types/props.type';
import { UserContext } from '../../../../contexts/user/userContext';

export default function AuthenticatedRoute({ element }: Props) {
  const { isAuthenticated } = useContext(UserContext);

  return isAuthenticated ? element : <Navigate to={routes.login} replace />;
}
