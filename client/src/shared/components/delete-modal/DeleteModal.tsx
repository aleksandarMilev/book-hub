import './DeleteModal.css';

import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaExclamationTriangle, FaTrashAlt } from 'react-icons/fa';

const DeleteModal: FC<{
  showModal: boolean;
  toggleModal: () => void;
  deleteHandler: () => void;
  title?: string;
  message?: string;
}> = ({ showModal, toggleModal, deleteHandler, title, message }) => {
  const { t } = useTranslation('common');

  const effectiveTitle = title ?? t('deleteModal.title');
  const effectiveMessage = message ?? t('deleteModal.message');

  return (
    <>
      {showModal && <div className="delete-backdrop" onClick={toggleModal} />}
      <div className={`delete-modal ${showModal ? 'show' : ''}`} role="dialog" aria-modal="true">
        <div className="delete-modal-content">
          <div className="delete-modal-icon">
            <FaExclamationTriangle />
          </div>
          <h3 className="delete-modal-title">{effectiveTitle}</h3>
          <p className="delete-modal-message">{effectiveMessage}</p>
          <div className="delete-modal-actions">
            <button className="delete-btn cancel" onClick={toggleModal}>
              {t('buttons.cancel')}
            </button>
            <button className="delete-btn confirm" onClick={deleteHandler}>
              <FaTrashAlt className="me-2" /> {t('buttons.delete')}
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default DeleteModal;

