import { useContext, useEffect, useState } from 'react'
import { useParams, useLocation } from 'react-router-dom'

import { baseUrl } from '../../../common/constants/api'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import ReviewItem from '../../book/book-details/review-item/ReviewItem'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './ReviewList.css'

export default function ReviewList() {
    const { bookId } = useParams()
    const { token } = useContext(UserContext)

    const location = useLocation()
    const bookTitle = location.state

    const [reviews, setReviews] = useState([])
    const [loading, setLoading] = useState(true)
    const [page, setPage] = useState(1) 
    const [totalPages, setTotalPages] = useState(1)

    const pageSize = 10

    const fetchData = async (pageNumber) => {
        setLoading(true)
        const resp = await fetch(`${baseUrl}${routes.review}/${bookId}?pageIndex=${pageNumber}&pageSize=${pageSize}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })

        const res = await resp.json()
        setReviews(res.items)
        setTotalPages(Math.ceil(res.totalItems / pageSize))
        setLoading(false)
    }

    useEffect(() => {
        fetchData(page)
    }, [bookId, token, page])

    const handleNextPage = () => {
        if (page < totalPages) {
            setPage(page + 1)
        }
    }

    const handlePreviousPage = () => {
        if (page > 1) {
            setPage(page - 1)
        }
    }

    if (loading) {
        return <DefaultSpinner />
    }

    if (!reviews || reviews.length === 0) {
        return <p>No reviews found.</p>
    }

    return (
        <div className='review-list'>
            <h1>{bookTitle}</h1>
            {reviews.map(r => (
                <ReviewItem key={r.id} review={r} onVote={() => fetchData(page)} />
            ))}

            <div className='pagination-controls'>
                <button onClick={handlePreviousPage} disabled={page === 1}>
                    Previous
                </button>
                <span>Page {page} of {totalPages}</span>
                <button onClick={handleNextPage} disabled={page === totalPages}>
                    Next
                </button>
            </div>
        </div>
    )
}
