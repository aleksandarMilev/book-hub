import type React from 'react';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/notification/api/api';
import type { ResourceType } from '@/features/notification/components/last-item/types/resourceType';
import { mapResourceRoute } from '@/features/notification/components/last-item/utils/utils';
import type { NotificationType } from '@/features/notification/types/notification';
import { useAuth } from '@/shared/stores/auth/auth';

export const useClickHandler = (
  notification: NotificationType,
  refetchNotifications: () => void | Promise<void>,
) => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const onClickHandler = useCallback(
    async (e: React.MouseEvent) => {
      e.preventDefault();
      if (!token) {
        return;
      }

      const controller = new AbortController();
      const base = mapResourceRoute(notification.resourceType as ResourceType);

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
