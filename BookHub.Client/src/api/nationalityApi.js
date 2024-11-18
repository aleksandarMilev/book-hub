import { baseUrl, routes } from '../common/constants/api'

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