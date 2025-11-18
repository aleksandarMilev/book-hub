import './DeleteModal.css';

import type { FC } from 'react';
import { FaExclamationTriangle, FaTrashAlt } from 'react-icons/fa';

const DeleteModal: FC<{
  showModal: boolean;
  toggleModal: () => void;
  deleteHandler: () => void;
  title?: string;
  message?: string;
}> = ({
  showModal,
  toggleModal,
  deleteHandler,
  title = 'Confirm Deletion',
  message = 'Are you sure you want to delete this item? This action cannot be undone.',
}) => {
  return (
    <>
      {showModal && <div className="delete-backdrop" onClick={toggleModal} />}
      <div className={`delete-modal ${showModal ? 'show' : ''}`} role="dialog" aria-modal="true">
        <div className="delete-modal-content">
          <div className="delete-modal-icon">
            <FaExclamationTriangle />
          </div>
          <h3 className="delete-modal-title">{title}</h3>
          <p className="delete-modal-message">{message}</p>
          <div className="delete-modal-actions">
            <button className="delete-btn cancel" onClick={toggleModal}>
              Cancel
            </button>
            <button className="delete-btn confirm" onClick={deleteHandler}>
              <FaTrashAlt className="me-2" /> Delete
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default DeleteModal;
