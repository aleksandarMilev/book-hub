import { format } from 'date-fns';
import { type FC } from 'react';
import { Dropdown } from 'react-bootstrap';

import { getIcon } from './utils/utils';
import type { NotificationType } from '../../../api/notification/types/notification';

import './LastNotificationsListItem.css';
import * as hooks from '../../../hooks/useNotification';

const LastNotificationsListItem: FC<{
  notification: NotificationType;
  refetchNotifications: () => void | Promise<void>;
}> = ({ notification, refetchNotifications }) => {
  const { onClickHandler } = hooks.useClickHandler(notification, refetchNotifications);

  const notificationClass = notification.isRead
    ? 'notification-item-read'
    : 'notification-item-unread';

  return (
    <Dropdown.Item key={notification.id} onClick={onClickHandler} className="notification-item">
      <div className="notification-header">
        <span className="notification-icon-wrapper">{getIcon(notification.resourceType)}</span>
        <strong className="resource-type">{notification.resourceType}:</strong>
      </div>
      <div className="notification-message">{notification.message}</div>
      <div className="notification-footer">
        <small className="notification-timestamp">
          {format(new Date(notification.createdOn), 'dd MMM yyyy')}
        </small>
      </div>
      <div className="notification-footer">
        <span className={`notification-status ${notificationClass}`}>
          {notification.isRead ? 'Read' : 'Unread'}
        </span>
      </div>
    </Dropdown.Item>
  );
};

export default LastNotificationsListItem;
