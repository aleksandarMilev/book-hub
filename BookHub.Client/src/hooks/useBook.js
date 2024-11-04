import { useEffect, useState } from 'react';

import * as bookApi from '../api/bookAPI'

export function useGetAll(){
    const [books, setBooks] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const abortController = new AbortController()

        async function fetchData() {
            setIsFetching(old => !old)
            setBooks(await bookApi.getAllAsync(abortController.signal))
            setIsFetching(old => !old)
        }

        fetchData()
        return () => abortController.abort()
    }, [])

    return { books, isFetching }
}

export function useGetDetails(id){
    const [book, setBook] = useState({})
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const abortController = new AbortController()

        async function fetchData() {
            setIsFetching(old => !old)
            setBook(await bookApi.getDetailsAsync(id, abortController.signal))
            setIsFetching(old => !old)
        }

        fetchData()
        return () => abortController.abort()
    }, [id])

    return { book, isFetching }
}
