export interface NotificationType {
  id: number;
  message: string;
  isRead: boolean;
  createdOn: string;
  resourceId: number;
  resourceType: string;
}
