import { useEffect, useState } from 'react';

import { getAllAsync, getDetailsAsync } from '.././api/bookAPI'

export function useGetAllBooks(){
    const [books, setBooks] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const abortController = new AbortController();

        async function fetchData() {
            setIsFetching(old => !old)
            setBooks(await getAllAsync(abortController.signal))
            setIsFetching(old => !old)
        };

        fetchData()
        return () => abortController.abort()
    }, [])

    return { books, isFetching }
}

export function useGetBookDetails(id){
    const [book, setBook] = useState({})
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const abortController = new AbortController();

        async function fetchData() {
            setIsFetching(old => !old)
            setBook(await getDetailsAsync(id, abortController.signal))
            setIsFetching(old => !old)
        };

        fetchData()
        return () => abortController.abort()
    }, [id])

    return { book, isFetching }
}
