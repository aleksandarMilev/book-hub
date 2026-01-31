import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

const AdminRoute: FC<JsxElementProps> = ({ element }: JsxElementProps) => {
  const { isAuthenticated, isAdmin } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!isAdmin) {
    return <Navigate to={routes.accessDenied} replace />;
  }

  return element;
};

export default AdminRoute;


