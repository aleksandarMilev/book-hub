import './LastNotifications.css';

import type { FC } from 'react';
import { Badge, Dropdown } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';
import { FaBell } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import LastNotificationsListItem from '@/features/notification/components/last-item/LastNotificationsListItem';
import { useLastThree } from '@/features/notification/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { routes } from '@/shared/lib/constants/api';

const LastNotifications: FC = () => {
  const { t } = useTranslation('notifications');

  const { notifications, isFetching, refetch } = useLastThree();
  const unreadCount = notifications.filter((n) => !n.isRead).length;

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <Dropdown align="end" className="bh-notifications-dd">
      <Dropdown.Toggle
        variant="light"
        id="notifications-dropdown"
        className="bh-notifications-dd__toggle"
      >
        <FaBell size={22} />
        {unreadCount > 0 && (
          <Badge pill bg="danger" className="bh-notifications-dd__badge">
            {unreadCount}
          </Badge>
        )}
      </Dropdown.Toggle>

      <Dropdown.Menu className="bh-notifications-dd__menu">
        <div className="bh-notifications-dd__header">
          <div className="bh-notifications-dd__title">{t('dropdown.title')}</div>
          <div className="bh-notifications-dd__subtitle">
            {unreadCount > 0
              ? t('dropdown.unreadCount', { count: unreadCount })
              : t('dropdown.allCaughtUp')}
          </div>
        </div>

        <div className="bh-notifications-dd__items">
          {notifications.length > 0 ? (
            notifications.map((n) => (
              <LastNotificationsListItem
                key={n.id}
                notification={n}
                refetchNotifications={refetch}
              />
            ))
          ) : (
            <Dropdown.Item className="bh-notifications-dd__empty">
              {t('dropdown.empty')}
            </Dropdown.Item>
          )}
        </div>

        <div className="bh-notifications-dd__footer">
          <Dropdown.Item as={Link} to={routes.notification} className="bh-notifications-dd__all">
            {t('dropdown.viewAll')}
          </Dropdown.Item>
        </div>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default LastNotifications;


