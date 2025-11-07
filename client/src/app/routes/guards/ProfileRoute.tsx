import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

export default function ProfileRoute({ element }: JsxElementProps) {
  const { isAuthenticated, hasProfile, isAdmin } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!hasProfile && !isAdmin) {
    return <Navigate to={routes.profile} replace />;
  }

  return element;
}
