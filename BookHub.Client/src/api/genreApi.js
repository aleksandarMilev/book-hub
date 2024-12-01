import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getGenresAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.genres
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.genre.namesBadRequest)
}

export async function detailsAsync(id, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.genres + `/${id}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.genre.details)
}