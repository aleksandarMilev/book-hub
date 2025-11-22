import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

const ProfileRoute: FC<JsxElementProps> = ({ element }: JsxElementProps) => {
  const { isAuthenticated, hasProfile, isAdmin } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!hasProfile && !isAdmin) {
    return <Navigate to={routes.profile} replace />;
  }

  return element;
};

export default ProfileRoute;
