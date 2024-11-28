import { Dropdown } from 'react-bootstrap'

import * as useNotification from '../../../../hooks/useNotification'

import DefaultSpinner from '../../default-spinner/DefaultSpinner'
import LastNotificationsListItem from '../last-notifications-list-item/LastNotificationsListItem'

import './LastNotifications.css'

export default function LastNotifications() {
    const { notifications, isFetching } = useNotification.useLastThree()

    if(isFetching){
        return <DefaultSpinner/>
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
