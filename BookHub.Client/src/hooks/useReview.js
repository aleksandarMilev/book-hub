import { useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as reviewApi from '../api/reviewApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useCreate() {
    const { token } = useContext(UserContext)

    const navigate = useNavigate()

    const createHandler = async (reviewData) => {
        const review = {
            ...reviewData,
            bookId: reviewData.bookId
        }

        try {
            const reviewId = await reviewApi.createAsync(review, token)
            return reviewId
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return createHandler
}

export function useEdit() {
    const { token } = useContext(UserContext)
    const navigate = useNavigate()

    const editHandler = async (reviewId, reviewData) => {
        const review = {
            ...reviewData,
            bookId: reviewData.bookId
        }

        try {
            const isSuccessful = await reviewApi.editAsync(reviewId, review, token)
            return isSuccessful
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return editHandler
}