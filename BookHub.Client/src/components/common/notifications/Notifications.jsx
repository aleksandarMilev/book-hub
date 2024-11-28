import { useState } from 'react'
import { Dropdown } from 'react-bootstrap'

import './Notifications.css'

export default function Notifications() {
    const [notifications] = useState([
        "New article posted: 'The Art of Storytelling'",
        "Your book review has received a reply!",
        "Admin approved your submitted book."
    ])

    return (
        <Dropdown align="end">
            <Dropdown.Toggle variant="light" id="notifications-dropdown">
                Notifications
            </Dropdown.Toggle>
            <Dropdown.Menu>
                {notifications.length > 0 ? (
                    notifications.map((notification, index) => (
                        <Dropdown.Item key={index}>
                            {notification}
                        </Dropdown.Item>
                    ))
                ) : (
                    <Dropdown.Item>No new notifications</Dropdown.Item>
                )}
            </Dropdown.Menu>
        </Dropdown>
    )
}
