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

export async function deleteAsync(id, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.review + `/${id}`
    const response = await fetch(url, options)

    if(response.ok){
        return true 
    }

    return false
}

export async function upvoteAsync(id, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            reviewId: id,
            isUpvote: true
        })
    }

    const url = baseUrl + routes.vote
    const response = await fetch(url, options)

    if(response.ok){
        return true 
    }

    return false
}

export async function downvoteAsync(id, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            reviewId: id,
            isUpvote: false
        })
    }

    const url = baseUrl + routes.vote
    const response = await fetch(url, options)

    if(response.ok){
        return true 
    }

    return false
}
