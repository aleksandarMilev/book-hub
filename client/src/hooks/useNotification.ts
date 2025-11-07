import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/notification/notificationApi';
import type { NotificationType } from '../api/notification/types/notification';
import { routes } from '../common/constants/api';
import { pagination } from '../common/constants/defaultValues';
import { errors } from '../common/constants/messages';
import { mapResourceRoute } from '../components/notifications/last-notifications-list-item/utils/utils';
import { useMessage } from '../contexts/message/messageContext';
import { UserContext } from '../contexts/user/userContext';


export function useLastThree() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [notifications, setNotifications] = useState<NotificationType[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);

        const data = await api.lastThree(token, signal);

        setNotifications(data ?? []);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Failed to load notifications.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  return { notifications, isFetching, refetch: fetchData };
}

export function useAll(page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [notifications, setNotifications] = useState<NotificationType[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);

        const result = await api.all(token, page, pageSize, signal);

        setNotifications(result.items ?? []);
        setTotalItems(result.totalItems ?? 0);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Failed to load notifications.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, page, pageSize, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  return { notifications, totalItems, isFetching, refetch: fetchData };
}

export function useClickHandler(
  notification: NotificationType,
  refetchNotifications: () => void | Promise<void>,
) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const onClickHandler = useCallback(
    async (e: React.MouseEvent) => {
      e.preventDefault();
      if (!token) {
        return;
      }

      const controller = new AbortController();

      try {
        await api.markAsRead(notification.id, token, controller.signal);
        await Promise.resolve(refetchNotifications());

        const base = mapResourceRoute(notification.resourceType);
        navigate(`${base}/${notification.resourceId}`);
      } finally {
        controller.abort();
      }
    },
    [token, notification, refetchNotifications, navigate],
  );

  return { onClickHandler };
}

export const useRemove = (id: number, refetch: () => void | Promise<void>) => {
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!showModal) {
      toggleModal();
      return;
    }

    if (!token) {
      return;
    }

    const controller = new AbortController();
    try {
      const success = await api.remove(id, token, controller.signal);
      if (success) {
        showMessage('Notification successfully deleted!', true);
        await Promise.resolve(refetch());
      } else {
        showMessage(errors.notification.delete, false);
      }
    } catch (error: unknown) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to delete notification.';
      showMessage(message, false);
    } finally {
      toggleModal();
    }

    return () => controller.abort();
  }, [showModal, token, id, refetch, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};
