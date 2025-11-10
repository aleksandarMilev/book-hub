import { format } from 'date-fns';
import { MDBBtn } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { FaTrashAlt } from 'react-icons/fa';

import * as hooks from '@/features/notifications/hooks/useCrud';
import type { NotificationType } from '@/features/notifications/types/notification';

const NotificationItem: FC<{
  notification: NotificationType;
  refetch: () => void | Promise<void>;
}> = ({ notification, refetch }) => {
  const { showModal, toggleModal, deleteHandler } = hooks.useRemove(notification.id, refetch);
  const { onClickHandler } = hooks.useClickHandler(notification, refetch);

  return (
    <li
      className={`list-group-item d-flex justify-content-between align-items-start ${
        notification.isRead ? 'read' : 'unread'
      }`}
    >
      <div className="notification-content">
        <h5 className="mb-1">
          <i className={`fa ${notification.resourceType === 'Book' ? 'fa-book' : 'fa-user'}`} />{' '}
          {notification.resourceType}
        </h5>
        <p className="mb-1 text-muted">{notification.message}</p>
        <small className="text-secondary">
          {format(new Date(notification.createdOn), 'dd MMM yyyy')}
        </small>
        <p className="text-secondary">{notification.isRead ? 'Read' : 'Unread'}</p>
      </div>
      <div className="notification-actions">
        <button onClick={onClickHandler} className="btn btn-outline-primary btn-sm me-2">
          View <i className="fa fa-arrow-right" />
        </button>
        <MDBBtn outline color="danger" size="sm" onClick={toggleModal}>
          <FaTrashAlt className="me-1" /> Delete
        </MDBBtn>
      </div>
      <DeleteModal showModal={showModal} toggleModal={toggleModal} deleteHandler={deleteHandler} />
    </li>
  );
};

export default NotificationItem;
