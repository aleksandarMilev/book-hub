import { useEffect, useState } from 'react';

export default function useFetch(serviceFunction, initState, dependencies = []){
    const [data, setData] = useState(initState)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const abortController = new AbortController();

        const fetchData = async () => {
            setIsFetching(old => !old)
            const result = await serviceFunction(abortController.signal)
            setData(result)
            setIsFetching(old => !old)
        }

        fetchData()
        return () => abortController.abort()
    }, dependencies)

    return { data, isFetching }
}