import './LastNotifications.css';

import type { FC } from 'react';
import { Badge, Dropdown } from 'react-bootstrap';
import { FaBell } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import LastNotificationsListItem from '@/features/notification/components/last-item/LastNotificationsListItem.js';
import { useLastThree } from '@/features/notification/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import { routes } from '@/shared/lib/constants/api.js';

const LastNotifications: FC = () => {
  const { notifications, isFetching, refetch } = useLastThree();
  const unreadNotifications = notifications.filter((n) => !n.isRead);

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
          <Dropdown.Item as={Link} to={routes.notification}>
            All
          </Dropdown.Item>
        </Link>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default LastNotifications;
