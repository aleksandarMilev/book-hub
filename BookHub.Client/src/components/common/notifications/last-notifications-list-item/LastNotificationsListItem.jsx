import { Dropdown } from 'react-bootstrap'
import { format } from 'date-fns'
import { FaBook, FaUser } from 'react-icons/fa'
 
import './LastNotificationsListItem.css'

export default function LastNotificationsListItem({ notification }){
    const getIcon = (resourceType) => {
        switch (resourceType) {
            case 'Book':
                return <FaBook className="notification-icon" />
            case 'Author':
                return <FaUser className="notification-icon" />
        }
    }

    const notificationClass = notification.isRead ? "notification-item-read" : "notification-item-unread"
    const readStatusMessage = notification.isRead ? "Read" : "Unread"

    return (
        <Dropdown.Item key={notification.id} className="notification-item">
            <div className="notification-header">
                <span className="notification-icon-wrapper">
                    {getIcon(notification.resourceType)} 
                </span>
                <strong className="resource-type">{notification.resourceType}:</strong>
            </div>
            <div className="notification-message">{notification.message}</div>
            <div className="notification-footer">
                <small className="notification-timestamp">{format(new Date(notification.createdOn), "dd MMM yyyy")}</small>
            </div>
            <div className="notification-footer">
                <span className={`notification-status ${notificationClass}`}>{readStatusMessage}</span>
            </div>
        </Dropdown.Item>
    )
}