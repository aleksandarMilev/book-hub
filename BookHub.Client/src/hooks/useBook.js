import { useContext, useEffect, useState } from 'react';

import * as bookApi from '../api/bookApi'
import * as authorApi from '../api/authorApi'
import { UserContext } from '../contexts/userContext';

export function useGetAll(){
    const { token } = useContext(UserContext)
    const [books, setBooks] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setBooks(await bookApi.getAllAsync(token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [])

    return { books, isFetching }
}

export function useGetFullInfo(id){
    const { token } = useContext(UserContext)
    const [book, setBook] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setBook(await bookApi.getDetailsAsync(id, token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [id])

    return { book, isFetching }
}

export function useCreate(){
    const { token } = useContext(UserContext) 

    const createHandler = async (bookData) => {
        const book = {
            ...bookData,
            imageUrl: bookData.imageUrl || null,
            authorId: bookData.authorId || null,
            authorName: bookData.authorName || null,
            publishedDate: bookData.publishedDate || null
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

export function useEdit(){
    const { token } = useContext(UserContext) 

    const editHandler = async (bookId, bookData) => {
        const book = {
            ...bookData,
            imageUrl: bookData.imageUrl || null,
            authorName: bookData.authorName || null,
            publishedDate: bookData.publishedDate || null
        }

        try {
            await bookApi.editAsync(bookId, book, token)
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return editHandler
}


export function useGetTopThree(){
    const { token } = useContext(UserContext)
    const [books, setBooks] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setBooks(await bookApi.getTopThreeAsync(token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [])

    return { books, isFetching }
}

