import { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'

import * as notificationApi from '../api/notificationApi'
import { pagination } from '../common/constants/defaultValues'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useLastThree(){
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [notifications, setNotifications] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    const fetchData = async () => {
        try {
            setIsFetching(true)
            setNotifications(await notificationApi.lastThreeAsync(token))
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } } )
        } finally {
            setIsFetching(false)
        }
    }

    useEffect(() => {
        
        fetchData()
    }, [token, navigate])

    return { notifications, isFetching, refetch: fetchData}
}

export function useAll(page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize){
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [notifications, setNotifications] = useState([])
    const [totalItems, setTotalItems] = useState(0)
    const [isFetching, setIsFetching] = useState(false)

    const fetchData = async () => {
        try {
            setIsFetching(true)
            const result = await notificationApi.allAsync(token, page, pageSize) 
            setNotifications(result.items)
            setTotalItems(result.totalItems)
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } } )
        } finally {
            setIsFetching(false)
        }
    }

    useEffect(() => {
        fetchData()
    }, [token, page, pageSize, navigate])

    return { notifications, totalItems, isFetching, refetch: fetchData}
}
