import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig } from '../common/utils';
import type { NotificationType } from './types/notification';
import type { PagedNotifications } from './types/pageNotification';

export async function lastThree(token: string) {
  try {
    const url = `${baseUrl}${routes.lastThreeNotifications}`;
    const response = await axios.get<NotificationType[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.notification.lastThree);
  }
}

export async function all(token: string, pageIndex: number, pageSize: number) {
  try {
    const url = `${baseUrl}${routes.notification}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const response = await axios.get<PagedNotifications>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.notification.all);
  }
}

export async function markAsRead(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.notification}/${id}/read`;
    await axios.patch(url, null, getAuthConfig(token));
  } catch {
    throw new Error(errors.notification.markAsRead);
  }
}

export async function remove(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.notification}/${id}`;
    await axios.delete(url, getAuthConfig(token));

    return true;
  } catch {
    return false;
  }
}
