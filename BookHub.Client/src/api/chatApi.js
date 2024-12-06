import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function addUserToChatAsync(chatId, userId, token){
    const optionsOjb = {
        userId
    }

    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(optionsOjb)
    }
    const url = baseUrl + routes.chat + `/${chatId}/users`
    const response = await fetch(url, options)

    console.log(JSON.stringify(userId))

    if(!response.ok){
        console.log(await response.json())
        
        throw new Error(errors.chat.addUser)
    }
}

export async function chatsNotJoinedAsync(userId, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    }
    const url = baseUrl + routes.chatsNotJoined + `/?userId=${userId}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.chat.names)
}

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