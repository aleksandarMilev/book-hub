import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export default function AuthenticatedRoute({ element }: JsxElementProps) {
  const { isAuthenticated } = useAuth();

  return isAuthenticated ? element : <Navigate to={routes.login} replace />;
}
