import { baseUrl, routes } from '../common/constants/api'

export async function getGenresAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.genres

    try {
        const response = await fetch(url, options)

        if(!response.ok){
            throw new Error()
        }

        return await response.json()
    } catch {
        throw new Error()
    }
}