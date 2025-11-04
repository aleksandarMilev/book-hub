import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/notification/notificationApi';
import { routes } from '../common/constants/api';
import { pagination } from '../common/constants/defaultValues';
import { UserContext } from '../contexts/user/userContext';
import type { NotificationType } from '../api/notification/types/notification';

export function useLastThree() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [notifications, setNotifications] = useState<NotificationType[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token) return;

    try {
      setIsFetching(true);
      const data = await api.lastThree(token);
      setNotifications(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load notifications.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { notifications, isFetching, refetch: fetchData };
}

export function useAll(page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [notifications, setNotifications] = useState<NotificationType[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token) {
      return;
    }

    try {
      setIsFetching(true);
      const result = await api.all(token, page, pageSize);

      setNotifications(result.items);
      setTotalItems(result.totalItems);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load notifications.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [token, page, pageSize, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { notifications, totalItems, isFetching, refetch: fetchData };
}
