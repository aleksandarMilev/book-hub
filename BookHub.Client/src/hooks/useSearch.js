import { useState, useEffect, useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as api from '../api/searchApi'
import { pagination } from '../common/constants/defaultValues'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useBooks(searchTerm, page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [books, setBooks] = useState([])
    const [totalItems, setTotalItems] = useState(0)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                const result = await api.searchBooksAsync(token, searchTerm || '', page, pageSize)
                setBooks(result.items)
                setTotalItems(result.totalItems)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [searchTerm, page, pageSize, token, navigate])

    return { books, totalItems, isFetching }
}