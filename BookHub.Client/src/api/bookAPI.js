import { baseUrl, routes } from '../common/constants/api'

export async function getAllAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.books
    const response = await fetch(url, options)
    return response.ok ? await response.json() : null
}

export async function getDetailsAsync(id, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.books + `/${id}`
    const response = await fetch(url, options)
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

    console.log(JSON.stringify(book))

    const response = await fetch(baseUrl + routes.books, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const bookId = await response.json()
    return bookId
}

export async function editAsync(bookId, book, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(book)
    }

    const url = baseUrl + routes.books + `/${bookId}`
    await fetch(url, options)
}

export async function deleteAsync(bookId, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.books + `/${bookId}`
    await fetch(url, options)
}

export async function getTopThreeAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.topThreeBooks
    const response = await fetch(url, options)
    return response.ok ? await response.json() : null
}

