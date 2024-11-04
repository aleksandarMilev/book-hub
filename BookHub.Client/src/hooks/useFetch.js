import { useEffect, useState } from 'react';

export default function useFetch(serviceFunction, initState, dependencies = []){
    const [data, setData] = useState(initState)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const abortController = new AbortController();

        async function fetchData(){
            setIsFetching(old => !old)
            setData(await serviceFunction(abortController.signal))
            setIsFetching(old => !old)
        }

        fetchData()
        return () => abortController.abort()
    }, dependencies)

    return { data, isFetching }
}