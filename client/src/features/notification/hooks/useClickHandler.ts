import type React from 'react';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/notification/api/api.js';
import { mapResourceRoute } from '@/features/notification/components/last-item/utils/utils.js';
import type { NotificationType } from '@/features/notification/types/notification.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useClickHandler = (
  notification: NotificationType,
  refetchNotifications: () => void | Promise<void>,
) => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const onClickHandler = useCallback(
    async (event: React.MouseEvent) => {
      event.preventDefault();
      if (!token) {
        return;
      }

      const controller = new AbortController();
      const base = mapResourceRoute(notification.resourceType);

      try {
        await api.markAsRead(notification.id, token, controller.signal);
      } finally {
        controller.abort();

        await Promise.resolve(refetchNotifications());
        navigate(`${base}/${notification.resourceId}`);
      }
    },
    [token, notification, refetchNotifications, navigate],
  );

  return { onClickHandler };
};
