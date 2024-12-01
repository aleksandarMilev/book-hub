import { useState } from 'react'
import { FaArrowLeft, FaArrowRight } from 'react-icons/fa'

import { pagination } from '../../../common/constants/defaultValues'
import * as useNotification from '../../../hooks/useNotification'

import NotificationItem from '../notification-item/NotificationItem'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './NotificationList.css'

export default function NotificationList() {
    const [page, setPage] = useState(pagination.defaultPageIndex)
    const pageSize = pagination.defaultPageSize
    
    const { notifications, totalItems, isFetching, refetch } = useNotification.useAll(page, pageSize)

    const totalPages = Math.ceil(totalItems / pageSize)

    const handlePageChange = (newPage) => {
        setPage(newPage)
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
                            <NotificationItem key={n.id} notification={n} refetch={refetch}/>
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
