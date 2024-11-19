import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getNationalitiesAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const response = await fetch(baseUrl + routes.authorNationalities, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.nationality.namesBadRequest)
}