import type { NotificationType } from './notification';

export interface PagedNotifications {
  items: NotificationType[];
  totalItems: number;
}
