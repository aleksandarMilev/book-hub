import './LastNotifications.css';

import type { FC, MouseEvent } from 'react';
import { Badge, Dropdown } from 'react-bootstrap';
import { FaBell } from 'react-icons/fa';
import { Link, useNavigate } from 'react-router-dom';

import * as hooks from '@/features/notifications/hooks/useCrud';

import LastNotificationsListItem from '../last-notifications-list-item/LastNotificationsListItem';

const LastNotifications: FC = () => {
  const navigate = useNavigate();
  const { notifications, isFetching, refetch } = hooks.use();

  const unreadNotifications = notifications.filter((n) => !n.isRead);

  const onClickHandler = (e: MouseEvent) => {
    e.preventDefault();
    navigate(routes.notification);
  };

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <Dropdown align="end">
      <Dropdown.Toggle variant="light" id="notifications-dropdown">
        <FaBell size={24} />
        {unreadNotifications.length > 0 && (
          <Badge pill bg="danger" className="notification-badge">
            {unreadNotifications.length}
          </Badge>
        )}
      </Dropdown.Toggle>
      <Dropdown.Menu>
        {notifications.length > 0 ? (
          notifications.map((n) => (
            <LastNotificationsListItem key={n.id} notification={n} refetchNotifications={refetch} />
          ))
        ) : (
          <Dropdown.Item>No new notifications</Dropdown.Item>
        )}
        <Link to={routes.notification}>
          <Dropdown.Item onClick={onClickHandler}>All</Dropdown.Item>
        </Link>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default LastNotifications;
