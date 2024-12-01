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

export async function allAsync(token, pageIndex, pageSize) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    
    const url = `${baseUrl}${routes.allNotifications}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.notification.all)
}

export async function markAsReadAsync(id, token){
    const options = {
        method: "PATCH",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.notification + `/${id}` + '/markread'
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.notification.markAsRead)
    }
}

export async function deleteAsync(id, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.notification + `/${id}`
    const response = await fetch(url, options)
    
    if(response.ok){
        return true 
    }

    return false
}