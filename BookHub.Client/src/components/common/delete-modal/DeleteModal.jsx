import { FaExclamationTriangle, FaTrashAlt } from "react-icons/fa"

export default function DeleteModal({ showModal, toggleModal, deleteHandler }) {
    return (
        <div className={`modal fade ${showModal ? 'show d-block' : ''}`} tabIndex="-1" aria-labelledby="deleteModalLabel" aria-hidden={!showModal}>
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content">
                    <div className="modal-header bg-warning text-white">
                        <h5 className="modal-title" id="deleteModalLabel">
                            <FaExclamationTriangle className="me-2" /> Confirm Deletion
                        </h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" onClick={toggleModal}></button>
                    </div>
                    <div className="modal-body">
                        <p className="text-center">Are you sure you want to delete this? This action cannot be undone.</p>
                    </div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" data-bs-dismiss="modal" onClick={toggleModal}>Cancel</button>
                        <button type="button" className="btn btn-danger" onClick={deleteHandler}>
                            <FaTrashAlt className="me-2" /> Delete
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )
}
