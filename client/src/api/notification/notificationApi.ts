import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { http } from '../common/http';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import type { NotificationType } from './types/notification';
import type { PagedNotifications } from './types/pageNotification';

export async function lastThree(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.lastThreeNotifications}`;
    const response = await http.get<NotificationType[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.notification.lastThree);
    throw error;
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
    const response = await http.get<PagedNotifications>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.notification.all);
    throw error;
  }
}

export async function markAsRead(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.notification}/${id}/read`;
    await http.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.notification.markAsRead);
    throw error;
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.notification}/${id}`;
    await http.delete<void>(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    if ((error as any)?.name === 'CanceledError') {
      throw error;
    }

    return false;
  }
}
