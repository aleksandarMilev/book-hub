import { useEffect, useState } from 'react';
import { Navigate, useParams } from 'react-router-dom';

import * as api from '@/api/chat/chatApi';
import type { JsxElementProps } from '@/app/types/jsxElementProps';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

export default function ChatRoute({ element }: JsxElementProps) {
  const { id } = useParams<{ id: string }>();
  const [isLoading, setIsLoading] = useState(true);
  const [hasAccess, setHasAccess] = useState(false);

  const { token, isAuthenticated, userId } = useAuth();

  useEffect(() => {
    if (!id || !userId || !token) {
      setIsLoading(false);
      return;
    }

    api
      .hasAccess(Number(id), userId, token)
      .then((access: boolean) => setHasAccess(access))
      .finally(() => setIsLoading(false));
  }, [id, userId, token]);

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  if (!hasAccess && !isLoading) {
    return <Navigate to={routes.home} replace />;
  }

  return element;
}
