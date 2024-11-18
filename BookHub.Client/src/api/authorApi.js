import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getAuthorNamesAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.authorNames

    try {
        const response = await fetch(url, options)

        if(response.ok){
            return await response.json()
        }

        throw new Error()
    } catch {
        throw new Error()
    }
}

export async function createAsync(author, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    }

    const url = baseUrl + routes.author

    try {
        const response = await fetch(url, options)

        if(response.ok){
            return await response.json()
        }

        throw new Error()
    } catch {
        throw new Error()
    }
}

export async function getDetailsAsync(id, token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.author + `/${id}`

    try {
        const response = await fetch(url, options)

        if(response.ok){
            return await response.json()
        }

        throw new Error()
    } catch {
        throw new Error()
    }
}

export async function getTopThreeAsync(token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.topThreeAuthors

    try {
        const response = await fetch(url, options)

        if(response.ok){
            return await response.json()
        }

        throw new Error()
    } catch {
        throw new Error()
    }
}

export async function editAsync(authorId, author, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    }

    const url = baseUrl + routes.author + `/${authorId}`
    
    try {
        const response = await fetch(url, options)

        if(!response.ok){
            throw new Error()
        }

    } catch {
        throw new Error()
    }
}

export async function deleteAsync(id, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.author + `/${id}`
    try {
        const response = await fetch(url, options)

        if(!response.ok){
            throw new Error(errors.author.editError)
        }

    } catch {
        throw new Error(errors.author.deleteError)
    }
}
