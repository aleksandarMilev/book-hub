import type { NotificationType } from '@/features/notification/types/notification';
import { getAuthConfig, http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

export const lastThree = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.lastThreeNotifications}`;
    const { data } = await http.get<NotificationType[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.notification.lastThree);
  }
};

export const all = async (
  token: string,
  pageIndex: number,
  pageSize: number,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.notification}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const { data } = await http.get<PaginatedResult<NotificationType>>(
      url,
      getAuthConfig(token, signal),
    );

    return data;
  } catch (error) {
    processError(error, errors.notification.all);
  }
};

export const markAsRead = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.notification}/${id}/read`;
    await http.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.notification.markAsRead);
  }
};

export const remove = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.notification}/${id}`;
    await http.delete<void>(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    if (IsError(error) && IsCanceledError(error)) {
      throw error;
    }

    return false;
  }
};


