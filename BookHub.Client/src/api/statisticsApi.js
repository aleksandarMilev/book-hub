import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getAsync() {
    const options = {
        method: "GET"
    }

    const url = baseUrl + routes.statistics
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.statistics.get)
}