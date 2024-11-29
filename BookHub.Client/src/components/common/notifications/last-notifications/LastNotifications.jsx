import { Link } from 'react-router-dom'
import { Dropdown } from 'react-bootstrap'

import * as useNotification from '../../../../hooks/useNotification'
import { routes } from '../../../../common/constants/api'

import DefaultSpinner from '../../default-spinner/DefaultSpinner'
import LastNotificationsListItem from '../last-notifications-list-item/LastNotificationsListItem'

import './LastNotifications.css'

export default function LastNotifications() {
    const { notifications, isFetching } = useNotification.useLastThree()

    if(isFetching){
        return <DefaultSpinner/>
    }

    const handleLinkClick = (e) => {
        e.preventDefault()
        history.push(routes.home)
    }

    return (
        <Dropdown align="end">
            <Dropdown.Toggle variant="light" id="notifications-dropdown">
                Notifications
            </Dropdown.Toggle>
            <Dropdown.Menu>
                {notifications.length > 0 
                    ? (
                        notifications.map(n => (<LastNotificationsListItem key={n.id} notification={n}/>))
                    ) : (
                    <Dropdown.Item>No new notifications</Dropdown.Item>
                )}
            </Dropdown.Menu>
        </Dropdown>
    )
}
