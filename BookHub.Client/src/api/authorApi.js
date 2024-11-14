import { baseUrl, routes } from '../common/constants/api'

export async function createAsync(author, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(author)
    }

    console.log(JSON.stringify(author))
    const response = await fetch(baseUrl + routes.author, options)
    
    if(!response.ok){
        console.log(await response.json())
        throw new Error('Something went wrong, please try again!')
    }

    const authorId = await response.json()
    return authorId
}

export async function getAuthorNamesAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const response = await fetch(baseUrl + routes.authorNames, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const authorNames = await response.json()
    return authorNames
}

export async function getNationalitiesAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const response = await fetch(baseUrl + routes.authorNationalities, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const nationalities = await response.json()
    return nationalities
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

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const author = await response.json()
    return author
}

export async function getTopThreeAsync(token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.topThreeAuthors
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const authors = await response.json()
    return authors
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

    console.log(author)

    const url = baseUrl + routes.author + `/${authorId}`
    await fetch(url, options)
}

export async function deleteAsync(id, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.author + `/${id}`
    await fetch(url, options)
}
