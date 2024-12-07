import { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { format } from 'date-fns'
import { FaTrashAlt } from 'react-icons/fa'
import { MDBBtn } from 'mdb-react-ui-kit'

import { errors } from '../../../common/constants/messages'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'
import * as notificationApi from '../../../api/notificationApi'

import DeleteModal from '../../common/delete-modal/DeleteModal'

export default function NotificationItem({ notification, refetch }){
    const navigate = useNavigate()
    const { token } = useContext(UserContext)

    const onClickHandler = async (e, notification) => {
        e.preventDefault()
        await notificationApi.markAsReadAsync(notification.id, token)
        refetch()
        navigate(`/${notification.resourceType}/${notification.resourceId}`)
    }
    
    const [showModal, setShowModal] = useState(false) 
    const toggleModal = () => setShowModal(prev => !prev)
    
    const deleteHandler = async () => {
        if (showModal) {
            const success = await notificationApi.deleteAsync(notification.id, token)
            
            if(success){
                navigate(routes.notification)
                refetch()
                toggleModal()
            } else {
                navigate(routes.badRequest, { state: { message: errors.notification.delete } })
            }
        } else {
            toggleModal()  
        }
    }

    return(
        <li
            className={`list-group-item d-flex justify-content-between align-items-start ${
                notification.isRead ? 'read' : 'unread'
            }`}
        >
            <div className="notification-content">
                <h5 className="mb-1">
                    <i className={`fa ${notification.resourceType === 'Book' ? 'fa-book' : 'fa-user'}`}></i>{' '}
                    {notification.resourceType}
                </h5>
                <p className="mb-1 text-muted">{notification.message}</p>
                <small className="text-secondary">
                    {format(new Date(notification.createdOn), "dd MMM yyyy")}
                </small>
                <p className="text-secondary">
                    {notification.isRead ? 'Read' : 'Unread'}
                </p>
            </div>
            <div className="notification-actions">
                <button
                    onClick={(e) => onClickHandler(e, notification)}
                    className="btn btn-outline-primary btn-sm me-2"
                >
                    View <i className="fa fa-arrow-right"></i>
                </button>
                <MDBBtn
                    outline
                    color="danger"
                    size="sm"
                    onClick={toggleModal}
                >
                    <FaTrashAlt className="me-1" /> Delete
                </MDBBtn>
            </div>
            <DeleteModal 
                showModal={showModal} 
                toggleModal={toggleModal} 
                deleteHandler={deleteHandler} 
            />
        </li>
    )
}
