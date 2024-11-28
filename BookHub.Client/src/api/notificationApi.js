import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function lastThreeAsync(token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.lastThreeNotifications
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.notification.lastThree)
}