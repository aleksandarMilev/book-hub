import { useState, useEffect, useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as api from '../api/searchApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useBooks(searchTerm) {
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [books, setBooks] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(old => !old)
                setBooks(await api.searchBooksAsync(searchTerm || '', token))
                setIsFetching(old => !old)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            }
        }

        fetchData()
    }, [searchTerm, token, navigate])

    return { books, isFetching }
}