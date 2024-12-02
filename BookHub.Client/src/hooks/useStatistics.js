import { useEffect, useState } from 'react'

import * as api from '../api/statisticsApi'

export function useGet(){
    const [statistics, setStatistics] = useState({})
    const [isFetching, setIsFetching] = useState(false)
    const [error, setError] = useState(null) 

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                setStatistics(await api.getAsync())
            } catch (error) {
                setError(error.message)
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [])

    return { statistics, isFetching, error }
}