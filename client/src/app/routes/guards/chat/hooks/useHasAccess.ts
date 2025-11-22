import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import * as api from '@/features/chat/api/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useHasAccess = () => {
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

  return { isAuthenticated, hasAccess, isLoading };
};
