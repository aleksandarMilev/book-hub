import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export default function AdminRoute({ element }: JsxElementProps) {
  const { isAuthenticated, isAdmin } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!isAdmin) {
    return <Navigate to={routes.accessDenied} replace />;
  }

  return element;
}
