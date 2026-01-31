import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import { useHasAccess } from '@/app/routes/guards/chat/hooks/useHasAccess';
import type { JsxElementProps } from '@/app/types/jsxElementProps';
import { routes } from '@/shared/lib/constants/api';

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


