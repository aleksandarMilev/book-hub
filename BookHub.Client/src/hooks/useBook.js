import { useContext, useEffect, useState } from 'react';

import * as bookApi from '../api/bookApi'
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

export function useGetDetails(id){
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
    const { token, userId } = useContext(UserContext) 

    const createHandler = async (title, author, description, imageUrl) => {
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

export function useEdit(){
    const { token, userId } = useContext(UserContext) 

    const editHandler = async (bookId, title, author, imageUrl ,description) => {
        const book = {
            title,
            author,
            description,
            imageUrl,
            userId
        }

        try {
            await bookApi.editAsync(bookId, book, token)
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return editHandler
}
