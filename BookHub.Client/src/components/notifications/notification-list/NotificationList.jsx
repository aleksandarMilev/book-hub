import { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { format } from 'date-fns'
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa'

import { pagination } from '../../../common/constants/defaultValues'
import { UserContext } from '../../../contexts/userContext'
import * as useNotification from '../../../hooks/useNotification'
import * as notificationApi from '../../../api/notificationApi'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './NotificationList.css'

export default function NotificationList() {
    const navigate = useNavigate()
    const { token } = useContext(UserContext)

    const [page, setPage] = useState(pagination.defaultPageIndex)
    const pageSize = pagination.defaultPageSize
    
    const { notifications, totalItems, isFetching, refetch } = useNotification.useAll(page, pageSize)

    const totalPages = Math.ceil(totalItems / pageSize)

    const handlePageChange = (newPage) => {
        setPage(newPage)
    }

    const onClickHandler = async (e, notification) => {
        e.preventDefault()
        await notificationApi.markAsReadAsync(notification.id, token)
        refetch()
        navigate(`/${notification.resourceType}/${notification.resourceId}`)
    }

    if (isFetching) {
        return <DefaultSpinner/>
    }

    return (
        <div className="container notification-list mt-5">
            <h2 className="text-primary mb-4">
                <i className="fa fa-bell"></i> Notifications
            </h2>
            {notifications && notifications.length > 0 ? (
                <>
                    <ul className="list-group shadow">
                        {notifications.map(n => (
                            <li
                                key={n.id}
                                className={`list-group-item d-flex justify-content-between align-items-start ${
                                    n.isRead ? 'read' : 'unread'
                                }`}
                            >
                                <div>
                                    <h5 className="mb-1">
                                        <i className={`fa ${n.resourceType === 'Book' ? 'fa-book' : 'fa-user'}`}></i>{' '}
                                        {n.resourceType}
                                    </h5>
                                    <p className="mb-1 text-muted">{n.message}</p>
                                    <small className="text-secondary">
                                        {format(new Date(n.createdOn), "dd MMM yyyy")}
                                    </small>
                                    <p className="text-secondary">
                                        {n.isRead ? 'Read' : 'Unread'}
                                    </p>
                                </div>
                                <button onClick={(e) => onClickHandler(e, n)} className="btn btn-outline-primary btn-sm">
                                    View <i className="fa fa-arrow-right"></i>
                                </button>
                            </li>
                        ))}
                    </ul>
                    <div className="pagination-container d-flex justify-content-center mt-4">
                        <button
                            className={`btn pagination-btn ${page === 1 ? 'disabled' : ''}`}
                            onClick={() => handlePageChange(page - 1)}
                            disabled={page === 1}
                        >
                            <FaArrowLeft /> Previous
                        </button>
                        <div className="pagination-info">
                            <span className="current-page">{page}</span> / <span className="total-pages">{totalPages}</span>
                        </div>
                        <button
                            className={`btn pagination-btn ${page === totalPages ? 'disabled' : ''}`}
                            onClick={() => handlePageChange(page + 1)}
                            disabled={page === totalPages}
                        >
                            Next <FaArrowRight />
                        </button>
                    </div>
                </>
            ) : (
                <div className="alert alert-info mt-4">No new notifications.</div>
            )}
        </div>
    )
}
