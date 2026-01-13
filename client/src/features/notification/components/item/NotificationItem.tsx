import './NotificationItem.css';
import { createPortal } from 'react-dom';

import { format } from 'date-fns';
import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaBook, FaComments, FaTrashAlt, FaUser } from 'react-icons/fa';

import { useClickHandler } from '@/features/notification/hooks/useClickHandler.js';
import { useRemove } from '@/features/notification/hooks/useCrud.js';
import type { NotificationType } from '@/features/notification/types/notification.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { getResourceType } from '../../utils/utils.js';

const getTypeIcon = (resourceType: string) => {
  switch (resourceType) {
    case 'Book':
      return <FaBook />;
    case 'Author':
      return <FaUser />;
    case 'Chat':
      return <FaComments />;
    default:
      return <FaUser />;
  }
};

const NotificationItem: FC<{
  notification: NotificationType;
  refetch: () => void | Promise<void>;
}> = ({ notification, refetch }) => {
  const { t } = useTranslation('notifications');

  const resourceType = getResourceType(notification.resourceType);
  const { showModal, toggleModal, deleteHandler } = useRemove(notification.id, refetch);
  const { onClickHandler } = useClickHandler(notification, refetch);

  const itemClass = `bh-notifications-item ${
    notification.isRead ? 'bh-notifications-item--read' : 'bh-notifications-item--unread'
  }`;

  return (
    <li className={itemClass}>
      <div className="bh-notifications-item__main">
        <div className="bh-notifications-item__top">
          <div className="bh-notifications-item__type">
            <span className="bh-notifications-item__typeIcon" aria-hidden="true">
              {getTypeIcon(resourceType)}
            </span>
            <span className="bh-notifications-item__typeText">
              {t(`resourceType.${getResourceType(notification.resourceType)}`, {
                defaultValue: getResourceType(notification.resourceType),
              })}
            </span>
            {!notification.isRead && <span className="bh-notifications-item__dot" aria-hidden />}
          </div>

          <span
            className={`bh-notifications-item__pill ${
              notification.isRead
                ? 'bh-notifications-item__pill--read'
                : 'bh-notifications-item__pill--unread'
            }`}
          >
            {notification.isRead ? t('status.read') : t('status.unread')}
          </span>
        </div>

        <p className="bh-notifications-item__message">{notification.message}</p>

        <div className="bh-notifications-item__meta">
          <span className="bh-notifications-item__date">
            {format(new Date(notification.createdOn), 'dd MMM yyyy')}
          </span>
        </div>
      </div>

      <div className="bh-notifications-item__actions">
        <button
          type="button"
          onClick={onClickHandler}
          className="bh-notifications-item__btn bh-notifications-item__btn--view"
        >
          {t('actions.view')}
          <span className="bh-notifications-item__arrow" aria-hidden>
            →
          </span>
        </button>

        <button
          type="button"
          onClick={toggleModal}
          className="bh-notifications-item__btn bh-notifications-item__btn--delete"
        >
          <FaTrashAlt className="bh-notifications-item__deleteIcon" aria-hidden="true" />
          {t('actions.delete')}
        </button>
      </div>

      {typeof document !== 'undefined' &&
        createPortal(
          <DeleteModal
            showModal={showModal}
            toggleModal={toggleModal}
            deleteHandler={deleteHandler}
          />,
          document.body,
        )}
    </li>
  );
};

export default NotificationItem;
