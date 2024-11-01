import { useEffect, useState } from 'react';

export default function useFetch(useEffectCallback, initState, dependencies = []){
    const [data, setData] = useState(initState)
    const [isFetching, setIsFetching] = useState(true)

    const controller = new AbortController()
    const { signal } = controller

    useEffect(() => {
        (async () => {
            setData(await useEffectCallback(signal))
            setIsFetching(false)
        })()

        return () => controller.abort()
    }, dependencies)

    return { data, isFetching }
}