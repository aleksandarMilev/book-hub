import type { NotificationType } from '@/features/notification/types/notification';
import { getAuthConfig, http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

export async function lastThree(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.lastThreeNotifications}`;
    const response = await http.get<NotificationType[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.notification.lastThree);
  }
}

export async function all(
  token: string,
  pageIndex: number,
  pageSize: number,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.notification}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const response = await http.get<PaginatedResult<NotificationType>>(
      url,
      getAuthConfig(token, signal),
    );

    return response.data;
  } catch (error) {
    processError(error, errors.notification.all);
  }
}

export async function markAsRead(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.notification}/${id}/read`;
    await http.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.notification.markAsRead);
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.notification}/${id}`;
    await http.delete<void>(url, getAuthConfig(token, signal));

    return true;
  } catch (error: unknown) {
    if (IsError(error) && IsCanceledError(error)) {
      throw error;
    }

    return false;
  }
}
