import { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/notification/api/api';
import type { NotificationType } from '@/features/notification/types/notification';
import { routes } from '@/shared/lib/constants/api';
import { pagination } from '@/shared/lib/constants/defaultValues';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';

export const useLastThree = () => {
  const { token } = useAuth();
  const [notifications, setNotifications] = useState<NotificationType[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) return;

      try {
        setIsFetching(true);
        const data = await api.lastThree(token, signal);
        setNotifications(data ?? []);
      } catch (error) {
        if (IsCanceledError(error)) return;
      } finally {
        setIsFetching(false);
      }
    },
    [token],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { notifications, isFetching, refetch: fetchData };
};

export const useAll = (
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { t } = useTranslation('notifications');

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

        setNotifications(result?.items ?? []);
        setTotalItems(result?.totalItems ?? 0);
      } catch (error) {
        if (IsCanceledError(error)) return;

        const message = IsError(error) ? error.message : t('errors.loadAll');
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, page, pageSize, navigate, t],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { notifications, totalItems, isFetching, refetch: fetchData };
};

export const useRemove = (id: string, refetch: () => void | Promise<void>) => {
  const { token } = useAuth();
  const { showMessage } = useMessage();
  const { t } = useTranslation('notifications');

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
        showMessage(t('messages.deleteSuccess'), true);
        await Promise.resolve(refetch());
      } else {
        showMessage(t('errors.delete'), false);
      }
    } catch (error: unknown) {
      if (IsCanceledError(error)) return;

      const message = IsError(error) ? error.message : t('errors.delete');
      showMessage(message, false);
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [showModal, token, id, refetch, showMessage, toggleModal, t]);

  return { showModal, toggleModal, deleteHandler };
};


