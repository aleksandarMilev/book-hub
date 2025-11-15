import './LastNotificationsListItem.css';

import { format } from 'date-fns';
import { type FC } from 'react';
import { Dropdown } from 'react-bootstrap';

import type { ResourceType } from '@/features/notification/components/last-item/types/resourceType.js';
import { useClickHandler } from '@/features/notification/hooks/useClickHandler.js';
import type { NotificationType } from '@/features/notification/types/notification.js';

import { getIcon } from './utils/utils.js';

const LastNotificationsListItem: FC<{
  notification: NotificationType;
  refetchNotifications: () => void | Promise<void>;
}> = ({ notification, refetchNotifications }) => {
  const { onClickHandler } = useClickHandler(notification, refetchNotifications);

  return (
    <Dropdown.Item key={notification.id} onClick={onClickHandler} className="notification-item">
      <div className="notification-header">
        <span className="notification-icon-wrapper">
          {getIcon(notification.resourceType as ResourceType)}
        </span>
        <strong className="resource-type">{notification.resourceType}:</strong>
      </div>
      <div className="notification-message">{notification.message}</div>
      <div className="notification-footer">
        <small className="notification-timestamp">
          {format(new Date(notification.createdOn), 'dd MMM yyyy')}
        </small>
      </div>
      <div className="notification-footer">
        <span
          className={`notification-status ${
            notification.isRead ? 'notification-item-read' : 'notification-item-unread'
          }`}
        >
          {notification.isRead ? 'Read' : 'Unread'}
        </span>
      </div>
    </Dropdown.Item>
  );
};

export default LastNotificationsListItem;
