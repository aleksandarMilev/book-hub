import { useState, useEffect, useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as api from '../api/searchApi'
import { pagination } from '../common/constants/defaultValues'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useBooks(searchTerm, page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
    console.log('BY BOOKS!');
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

export function useAuthors(searchTerm, page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [authors, setAuthors] = useState([])
    const [totalItems, setTotalItems] = useState(0)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                const result = await api.searchAuthorsAsync(token, searchTerm || '', page, pageSize)
                setAuthors(result.items)
                setTotalItems(result.totalItems)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [searchTerm, page, pageSize, token, navigate])

    return { authors, totalItems, isFetching }
}

export function useProfiles(searchTerm, page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [profiles, setProfiles] = useState([])
    const [totalItems, setTotalItems] = useState(0)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                const result = await api.searchProfilesAsync(token, searchTerm || '', page, pageSize)
                setProfiles(result.items)
                setTotalItems(result.totalItems)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [searchTerm, page, pageSize, token, navigate])

    return { profiles, totalItems, isFetching }
}

export function useArticles(searchTerm, page = pagination.defaultPageIndex, pageSize = pagination.defaultPageSize) {
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [articles, setArticles] = useState([])
    const [totalItems, setTotalItems] = useState(0)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                const result = await api.searchArticlesAsync(token, searchTerm || '', page, pageSize)
                setArticles(result.items)
                setTotalItems(result.totalItems)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [searchTerm, page, pageSize, token, navigate])

    return { articles, totalItems, isFetching }
}