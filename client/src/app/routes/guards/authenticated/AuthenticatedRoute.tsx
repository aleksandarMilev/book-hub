import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

const AuthenticatedRoute: FC<JsxElementProps> = ({ element }: JsxElementProps) => {
  const { isAuthenticated } = useAuth();

  return isAuthenticated ? element : <Navigate to={routes.login} replace />;
};

export default AuthenticatedRoute;
