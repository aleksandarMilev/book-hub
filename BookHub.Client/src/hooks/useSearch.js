import { useState, useEffect, useContext } from 'react'

import * as api from '../api/searchApi'
import { UserContext } from '../contexts/userContext'

export function useBooks(searchTerm) {
    const { token } = useContext(UserContext)
    const [books, setBooks] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setBooks(await api.searchBooksAsync(searchTerm || '', token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [searchTerm, token])

    return { books, isFetching }
}