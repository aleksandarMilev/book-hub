import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

const AuthenticatedRoute: FC<JsxElementProps> = ({ element }: JsxElementProps) => {
  const { isAuthenticated } = useAuth();

  return isAuthenticated ? element : <Navigate to={routes.login} replace />;
};

export default AuthenticatedRoute;


