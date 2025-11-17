import type { NotificationType } from '@/features/notification/types/notification.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils.js';
import type { PaginatedResult } from '@/shared/types/paginatedResult.js';

export const lastThree = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.lastThreeNotifications}`;
    const { data } = await http.get<NotificationType[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.notification.lastThree);
  }
};

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
