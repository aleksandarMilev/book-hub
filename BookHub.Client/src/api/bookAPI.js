import { baseUrl, routes } from '../common/constants/api'

export async function getAllAsync(signal){
    const response = await fetch(baseUrl + routes.books, signal)
    return response.ok ? await response.json() : null
}

export async function getDetailsAsync(id, signal){
    const response = await fetch(baseUrl + routes.books + `/${id}`, signal)
    return response.ok ? await response.json() : null
}

export async function createAsync(book, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(book)
    }

    const response = await fetch(baseUrl + routes.books, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const bookId = await response.json()
    return bookId
}