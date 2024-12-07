import { Link, useNavigate } from 'react-router-dom'
import { Dropdown, Badge } from 'react-bootstrap'
import { FaBell } from 'react-icons/fa'

import * as useNotification from '../../../hooks/useNotification'
import { routes } from '../../../common/constants/api'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'
import LastNotificationsListItem from '../last-notifications-list-item/LastNotificationsListItem'

import './LastNotifications.css'

export default function LastNotifications() {
    const navigate = useNavigate()
    const { notifications, isFetching, refetch } = useNotification.useLastThree()

    const unreadNotifications = notifications.filter(n => !n.isRead)  

    const onClickHandler = async (e) => {
        e.preventDefault()
        navigate(routes.notification)
    }

    if (isFetching) {
        return <DefaultSpinner />
    }

    return (
        <Dropdown align="end">
            <Dropdown.Toggle variant="light" id="notifications-dropdown">
                <FaBell size={24} />
                {unreadNotifications.length > 0 && (
                    <Badge pill bg="danger" className="notification-badge">
                        {unreadNotifications.length}
                    </Badge>
                )}
            </Dropdown.Toggle>

            <Dropdown.Menu>
                {notifications.length > 0 ? (
                    notifications.map((n) => (
                        <LastNotificationsListItem
                            key={n.id}
                            notification={n}
                            refetchNotifications={refetch}
                        />
                    ))
                ) : (
                    <Dropdown.Item>No new notifications</Dropdown.Item>
                )}

                <Link to={routes.notification}>
                    <Dropdown.Item onClick={onClickHandler}>All</Dropdown.Item>
                </Link>
            </Dropdown.Menu>
        </Dropdown>
    )
}
