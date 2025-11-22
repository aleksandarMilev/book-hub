import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import { useHasAccess } from '@/app/routes/guards/chat/hooks/useHasAccess.js';
import type { JsxElementProps } from '@/app/types/jsxElementProps.js';
import { routes } from '@/shared/lib/constants/api.js';

const ChatRoute: FC<JsxElementProps> = ({ element }: JsxElementProps) => {
  const { isAuthenticated, hasAccess, isLoading } = useHasAccess();

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!hasAccess && !isLoading) {
    return <Navigate to={routes.home} replace />;
  }

  return element;
};

export default ChatRoute;
