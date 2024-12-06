import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function createAsync(chat, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(chat)
    }
    const url = baseUrl + routes.chat
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.chat.create)
}

export async function editAsync(chatId, chat, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(chat)
    }

    const url = baseUrl + routes.chat + `/${chatId}`
    const response = await fetch(url, options)

    if(response.ok){
        return true
    }

    throw new Error(errors.chat.edit)
}