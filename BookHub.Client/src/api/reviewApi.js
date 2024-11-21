import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function createAsync(review, token) {
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(review)
    }

    const url = baseUrl + routes.review
    const response = await fetch(url, options)

    if (response.ok) {
        return await response.json()
    }

    throw new Error(errors.review.create)
}

export async function editAsync(reviewId, review, token) {
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(review)
    }

    const url = baseUrl + routes.review + `/${reviewId}`
    const response = await fetch(url, options)

    if (response.ok) {
        return true 
    }

    throw new Error(errors.review.edit)
}