import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getTopThreeAsync(token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.topThreeAuthors
    const response = await fetch(url, options)

        if(response.ok){
            return await response.json()
        }

        throw new Error(errors.author.topThree)
}

export async function getAuthorNamesAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.authorNames
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }
    
    throw new Error(errors.author.namesBadRequest)
}

export async function getDetailsAsync(id, token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.author + `/${id}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }
    
    throw new Error(errors.author.notfound)
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
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.author.create)

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
    const response = await fetch(url, options)

    if(response.ok){
        return true
    }

    throw new Error(errors.author.edit)
}

export async function deleteAsync(id, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.author + `/${id}`
    const response = await fetch(url, options)

    if(response.ok){
        return true
    }

    return false
}
