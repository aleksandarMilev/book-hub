import { type FC, useState } from 'react';

import { pagination } from '../../../common/constants/defaultValues';
import * as hooks from '../../../hooks/useNotification';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import Pagination from '../../common/pagination/Pagination';

import NotificationItem from '../notification-item/NotificationItem';
import './NotificationList.css';

const NotificationList: FC = () => {
  const [page, setPage] = useState<number>(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const { notifications, totalItems, isFetching, refetch } = hooks.useAll(page, pageSize);

  const totalPages = Math.ceil(totalItems / pageSize) || 1;

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) return;
    setPage(newPage);
  };

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <div className="container notification-list mt-5">
      <h2 className="text-primary mb-4">
        <i className="fa fa-bell" /> Notifications
      </h2>
      {notifications && notifications.length > 0 ? (
        <>
          <ul className="list-group shadow">
            {notifications.map((n) => (
              <NotificationItem key={n.id} notification={n} refetch={refetch} />
            ))}
          </ul>
          <Pagination
            page={page}
            totalPages={totalPages}
            disabled={isFetching}
            onPageChange={handlePageChange}
          />
        </>
      ) : (
        <div className="alert alert-info mt-4">No new notifications.</div>
      )}
    </div>
  );
};

export default NotificationList;
