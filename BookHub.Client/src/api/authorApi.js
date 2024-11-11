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

    const response = await fetch(baseUrl + routes.author, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const authorId = await response.json()
    return authorId
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