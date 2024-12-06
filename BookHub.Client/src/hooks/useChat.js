import { useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as api from '../api/chatApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useCreate(){
    const { token } = useContext(UserContext) 
    
    const navigate = useNavigate()

    const createHandler = async (chatData) => {
        const chat = {
            ...chatData,
            imageUrl: chatData.imageUrl || null,
        }

        try {
            const chatId = await api.createAsync(chat, token)
            return chatId
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return createHandler
}

export function useEdit(){
    const { token } = useContext(UserContext) 

    const navigate = useNavigate()

    const editHandler = async (chatId, chatData) => {
        const chat = {
            ...chatData,
            imageUrl: chatData.imageUrl || null,
        }

        try {
            const isSuccessful = await api.editAsync(chatId, chat, token)
            return isSuccessful
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return editHandler
}
