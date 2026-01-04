// naming rule violation to avoid the JS built-in class name
export type NotificationType = {
  id: string;
  message: string;
  isRead: boolean;
  createdOn: string;
  resourceId: string;
  resourceType: number;
};
