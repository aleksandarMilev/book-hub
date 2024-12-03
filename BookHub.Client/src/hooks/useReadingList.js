import { useContext, useEffect, useState } from 'react'

import * as api from '../api/readingListApi'
import { UserContext } from '../contexts/userContext'

export function useGet(
    id, 
    status, 
    page = null, 
    pageSize = null, 
    isPrivate = false
){
    const { token, userId } = useContext(UserContext)

    const [readingList, setReadingList] = useState([])
    const [totalItems, setTotalItems] = useState(0)
    const [isFetching, setIsFetching] = useState(false)
    const [error, setError] = useState(null) 

    useEffect(() => {
        async function fetchData() {
            if(isPrivate && id !== userId){
                setReadingList([])
                setIsFetching(false)
                setError(null)
                return
            }

            try {
                setIsFetching(true)
                const result = await api.getAsync(id, token, status, page, pageSize)
                setReadingList(result.items)
                setTotalItems(result.totalItems)
            } catch (error) {
                setError(error.message)
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [id, token, page, pageSize, status, isPrivate, userId])

    return { readingList, totalItems, isFetching, error }
}