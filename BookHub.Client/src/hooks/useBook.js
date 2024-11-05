import { useContext, useEffect, useState } from 'react';

import * as bookApi from '../api/bookAPI'
import { UserContext } from '../contexts/userContext';

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

export function useCreate(){
    const { token } = useContext(UserContext) 

    const createHandler = async (title, author, description, imageUrl, userId) => {
        const book = {
            title,
            author,
            description,
            imageUrl,
            userId
        }

        try {
            const bookId = await bookApi.createAsync(book, token)
            return bookId
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return createHandler
}
