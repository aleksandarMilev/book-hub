import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function addInListAsync(bookId, status, token){
    const bodyObj = {
        bookId,
        status
    }

    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyObj)
    }

    const url = baseUrl + routes.readingList
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.readingList.add)
    }
}

export async function removeFromListAsync(bookId, status, token){
    const bodyObj = {
        bookId,
        status
    }

    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyObj)
    }

    const url = baseUrl + routes.readingList
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.readingList.add)
    }
}
