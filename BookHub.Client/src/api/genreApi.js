import { baseUrl, routes } from '../common/constants/api'

export async function getGenresAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.genres
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error('Something went wrong, please try again!')
    }

    const genres = await response.json()
    return genres
}