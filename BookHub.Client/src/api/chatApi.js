import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function inviteUserToChatAsync(chatId, userId, token){
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
    const url = baseUrl + routes.chat + `/${chatId}/invite`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.chat.addUser)
    }
}

export async function rejectAsync(chatId, userId, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.chat + `/${chatId}` + `/invite/${userId}/reject`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.chat.reject)
    }
}


export async function acceptAsync(chatId, userId, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.chat + `/${chatId}` + `/invite/${userId}/accept`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.chat.accept)
    }

}

export async function hasAccessAsync(chatId, userId, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    const url = baseUrl + routes.chat + `/${chatId}` + `/access/${userId}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.chat.details)
}

export async function userIsInvitedAsync(chatId, userId, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    const url = baseUrl + routes.chat + `/${chatId}` + `/invited/${userId}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.chat.details)
}

export async function detailsAsync(chatId, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    }
    const url = baseUrl + routes.chat + `/${chatId}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.chat.details)
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

export async function createMessageAsync(message, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(message)
    }

    console.log(JSON.stringify(message))

    const url = baseUrl + routes.chatMessage
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.chat.createMessage)
    }
}

export async function editMessageAsync(messageId, message, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(message)
    }

    const url = baseUrl + routes.chatMessage + `/${messageId}`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.chat.editMessage)
    }
}

export async function deleteMessageAsync(id, token){
    console.log(typeof(token));
    console.log(typeof(id));
    

    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    }

    const url = baseUrl + routes.chatMessage + `/${id}`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.chat.deleteMessage)
    }
}