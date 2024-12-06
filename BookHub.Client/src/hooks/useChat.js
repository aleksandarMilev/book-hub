import { useContext, useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'

import * as api from '../api/chatApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useChatsNotJoined(userId){
    const { token } = useContext(UserContext)
    
    const navigate = useNavigate()
    const [chatNames, setChatNames] = useState(null)
    const [error, setError] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    async function fetchData() {
        if(!userId){
            return
        }
        
        try {
            setIsFetching(true)
            setChatNames(await api.chatsNotJoinedAsync(userId, token))
        } catch(error) {
            setError(error.message)
        } finally {
            setIsFetching(false)
        }
    }

    useEffect(() => {
        fetchData()
    }, [userId, token, navigate])

    return { chatNames, isFetching, error, refetch: fetchData  }
}

export function useDetails(chatId){
    const { token } = useContext(UserContext)
    
    const navigate = useNavigate()
    const [chat, setChat] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    async function fetchData() {
        try {
            setIsFetching(true)
            setChat(await api.detailsAsync(chatId, token))
        } catch(error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        } finally {
            setIsFetching(false)
        }
    }

    useEffect(() => {
        fetchData()
    }, [chatId, token, navigate])

    return { chat, isFetching, refetch: fetchData }
}


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

export function useCreateMessage(){
    const { token } = useContext(UserContext) 

    const createHandler = async (messageData) => {
        console.log(messageData);
        

        const message = {
            ...messageData
        }

        try {
            await api.createMessageAsync(message, token)
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return createHandler
}

export function useEditMessage(){
    const { token } = useContext(UserContext) 
    
    const editHandler = async (messageId, messageData) => {
        const message = {
            ...messageData
        }

        try {
            await api.editMessageAsync(messageId, message, token)
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return editHandler
}
