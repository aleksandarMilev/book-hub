import { Navigate } from 'react-router-dom';

import type { JsxElementProps } from '@/app/types/jsxElementProps';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

export default function AuthenticatedRoute({ element }: JsxElementProps) {
  const { isAuthenticated } = useAuth();

  return isAuthenticated ? element : <Navigate to={routes.login} replace />;
}
