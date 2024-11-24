import { useContext, useEffect, useState } from "react"
import { useParams } from "react-router-dom"

import { baseUrl } from "../../../common/constants/api"
import { routes } from '../../../common/constants/api'
import { UserContext } from "../../../contexts/userContext"

import ReviewItem from "../../book/book-details/review-item/ReviewItem"
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner"

export default function ReviewList() {
    const { token } = useContext(UserContext)
    const { bookId } = useParams()
    const [reviews, setReviews] = useState([])
    const [loading, setLoading] = useState(true)

    const fetchData = async () => {
        setLoading(true)  
        const resp = await fetch(baseUrl + routes.review + `/${bookId}`, {
            method: "GET",
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })

        const res = await resp.json()
        setReviews(res)
        setLoading(false)  
    }

    useEffect(() => {
        fetchData()
    }, [bookId, token])

    if (loading || !reviews || reviews.length === 0) {
        return <DefaultSpinner />
    }

    return (
        reviews.items.map((r) => (
            <ReviewItem key={r.id} review={r} onVote={fetchData} />
        ))
    )
}
