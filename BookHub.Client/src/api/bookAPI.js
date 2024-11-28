import { baseAdminUrl, baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getTopThreeAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.topThreeBooks
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.book.topThree)
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

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.book.notfound)
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

    const url = baseUrl + routes.books
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.book.create)
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
    const response = await fetch(url, options)

    if(response.ok){
        return true
    }

    throw new Error(errors.book.edit)
}

export async function deleteAsync(bookId, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.books + `/${bookId}`
    const response = await fetch(url, options)

    if(response.ok){
        return true 
    }

    return false
}

export async function approveAsync(bookId, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseAdminUrl + routes.admin.approveBook + `/${bookId}`
    const response = await fetch(url, options)

    if(response.ok){
        return true 
    }

    return false
}

export async function rejectAsync(bookId, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseAdminUrl + routes.admin.rejectBook + `/${bookId}`
    const response = await fetch(url, options)

    if(response.ok){
        return true 
    }

    return false
}
