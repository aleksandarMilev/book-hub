import { useContext, useEffect, useState } from 'react'

import * as api from '../api/readingListApi'
import { UserContext } from '../contexts/userContext'

export function useCurrentlyReadingList(isPrivate, id){
    const { token, userId } = useContext(UserContext)

    const [readingList, setReadingList] = useState([])
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
                setReadingList(await api.currentlyReadingListAsync(id, token))
            } catch (error) {
                setError(error.message)
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [id, token])

    return { readingList, isFetching, error }
}