import './LastNotificationsListItem.css';

import { format } from 'date-fns';
import { type FC } from 'react';
import { Dropdown } from 'react-bootstrap';

import { useClickHandler } from '@/features/notification/hooks/useClickHandler.js';
import type { NotificationType } from '@/features/notification/types/notification.js';

import { getIcon } from './utils/utils.js';
import type { ResourceType } from './types/resourceType.js';

type Props = {
  notification: NotificationType;
  refetchNotifications: () => void | Promise<void>;
};

const LastNotificationsListItem: FC<Props> = ({ notification, refetchNotifications }) => {
  const { onClickHandler } = useClickHandler(notification, refetchNotifications);

  return (
    <Dropdown.Item
      key={notification.id}
      onClick={onClickHandler}
      className={`bh-notifications-dd-item ${
        notification.isRead ? 'bh-notifications-dd-item--read' : 'bh-notifications-dd-item--unread'
      }`}
    >
      <div className="bh-notifications-dd-item__row">
        <span className="bh-notifications-dd-item__icon" aria-hidden="true">
          {getIcon(notification.resourceType as ResourceType)}
        </span>

        <div className="bh-notifications-dd-item__body">
          <div className="bh-notifications-dd-item__top">
            <strong className="bh-notifications-dd-item__type">{notification.resourceType}</strong>
            <span className="bh-notifications-dd-item__date">
              {format(new Date(notification.createdOn), 'dd MMM yyyy')}
            </span>
          </div>

          <div className="bh-notifications-dd-item__message">{notification.message}</div>

          <div className="bh-notifications-dd-item__meta">
            <span
              className={`bh-notifications-dd-item__pill ${
                notification.isRead
                  ? 'bh-notifications-dd-item__pill--read'
                  : 'bh-notifications-dd-item__pill--unread'
              }`}
            >
              {notification.isRead ? 'Read' : 'Unread'}
            </span>
          </div>
        </div>
      </div>
    </Dropdown.Item>
  );
};

export default LastNotificationsListItem;
