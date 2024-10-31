import { useEffect, useState } from 'react';

export default function useFetch(useEffectCallback, initState, dependencyArr = []){
    const [data, setData] = useState(initState)
    const [isFetching, setIsFetching] = useState(true)

    useEffect(() => {
        (async () => {
            setData(await useEffectCallback())
            setIsFetching(false)
        })()
    }, dependencyArr)

    return { data, isFetching }
}