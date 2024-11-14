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
    const { token } = useContext(UserContext) 

    const editHandler = async (bookId, title, author, imageUrl, description) => {
        const book = {
            title,
            author,
            description,
            imageUrl
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

export function useGenres() {
    const { token } = useContext(UserContext)
    const [genres, setGenres] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setGenres(await bookApi.getGenresAsync(token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [token])

    return { genres, isFetching }
}

export function useSearchGenres(genres, selectedGenres) {
    const [searchTerm, setSearchTerm] = useState('')
    const [filteredGenres, setFilteredGenres] = useState([])

    useEffect(() => {
        if (searchTerm === '') {
            setFilteredGenres([])
        } else {
            const filtered = genres.filter(
                g => g.toLowerCase().includes(searchTerm.toLowerCase()) && !selectedGenres.includes(g)
            )
            
            setFilteredGenres(filtered)
        }
    }, [searchTerm, genres, selectedGenres])

    const updateSearchTerm = (term) => setSearchTerm(term)

    return { searchTerm, filteredGenres, updateSearchTerm }
}