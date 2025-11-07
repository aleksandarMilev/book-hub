import React, { useContext, useState, useEffect } from 'react';
import { Navigate, useParams } from 'react-router-dom';

import * as api from '../../../../api/chat/chatApi';
import { routes } from '../../../../common/constants/api';
import { UserContext } from '../../../../contexts/user/userContext';

import type { Props } from '../types/props.type';

export default function ChatRoute({ element }: Props) {
  const { id } = useParams<{ id: string }>();
  const [isLoading, setIsLoading] = useState(true);
  const [hasAccess, setHasAccess] = useState(false);

  const { token, isAuthenticated, userId } = useContext(UserContext);

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

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

  if (!hasAccess && !isLoading) {
    return <Navigate to={routes.home} replace />;
  }

  return element;
}
