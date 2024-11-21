import { useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as reviewApi from '../api/reviewApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'
import { errors } from '../common/constants/messages'

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

export function useUpvote(){
    const { token } = useContext(UserContext)

    const upvoteHandler = async (id, setUpvoteCount, refreshReviews) => {
        const success = await reviewApi.upvoteAsync(id, token)

        if(success){
            setUpvoteCount(old => ++old)
            refreshReviews()
        } else {
            return
        }
    }

    return upvoteHandler
}

export function useDownvote(){
    const { token } = useContext(UserContext)

    const downvoteHandler = async (id, setDownvoteCount, refreshReviews) => {
        const success = await reviewApi.downvoteAsync(id, token)

        if(success){
            setDownvoteCount(old => ++old)
            refreshReviews()
        } else {
            return
        }
    }

    return downvoteHandler
}