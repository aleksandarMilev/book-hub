// naming rule violation to avoid the JS built-in class name
export type NotificationType = {
  id: number;
  message: string;
  isRead: boolean;
  createdOn: string;
  resourceId: number;
  resourceType: string;
};
