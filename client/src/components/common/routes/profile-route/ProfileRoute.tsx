import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';

import { routes } from '../../../../common/constants/api';
import type { Props } from '../types/props.type';
import { UserContext } from '../../../../contexts/user/userContext';

export default function ProfileRoute({ element }: Props) {
  const { isAuthenticated, hasProfile, isAdmin } = useContext(UserContext);

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!hasProfile && !isAdmin) {
    return <Navigate to={routes.profile} replace />;
  }

  return element;
}
