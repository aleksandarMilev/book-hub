import './NotificationList.css';

import { type FC, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { FaBell } from 'react-icons/fa';

import { useAll } from '@/features/notification/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';
import { pagination } from '@/shared/lib/constants/defaultValues';

import NotificationItem from '../item/NotificationItem.js';

const NotificationList: FC = () => {
  const { t } = useTranslation('notifications');

  const [page, setPage] = useState<number>(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const { notifications, totalItems, isFetching, refetch } = useAll(page, pageSize);
  const totalPages = Math.ceil(totalItems / pageSize) || 1;

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) return;
    setPage(newPage);
  };

  if (isFetching) {
    return <DefaultSpinner />;
  }

  const hasNotifications = notifications && notifications.length > 0;

  return (
    <div className="bh-notifications-page container">
      <div className="bh-notifications-header">
        <div className="bh-notifications-header__titleRow">
          <span className="bh-notifications-header__icon" aria-hidden="true">
            <FaBell />
          </span>
          <h1 className="bh-notifications-header__title">{t('page.title')}</h1>
        </div>
        <p className="bh-notifications-header__subtitle">{t('page.subtitle')}</p>
      </div>

      {hasNotifications ? (
        <>
          <ul className="bh-notifications-list" role="list">
            {notifications.map((n) => (
              <NotificationItem key={n.id} notification={n} refetch={refetch} />
            ))}
          </ul>

          <div className="bh-notifications-pagination">
            <Pagination
              page={page}
              totalPages={totalPages}
              disabled={isFetching}
              onPageChange={handlePageChange}
            />
          </div>
        </>
      ) : (
        <div className="bh-notifications-empty">
          <div className="bh-notifications-empty__card">
            <div className="bh-notifications-empty__title">{t('empty.title')}</div>
            <div className="bh-notifications-empty__text">{t('empty.text')}</div>
          </div>
        </div>
      )}
    </div>
  );
};

export default NotificationList;


