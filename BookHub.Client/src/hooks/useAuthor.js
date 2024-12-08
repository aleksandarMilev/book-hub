import { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'

import * as authorApi from '../api/authorApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useGetTopThree(){
    const { token } = useContext(UserContext)

    const [authors, setAuthor] = useState([])
    const [isFetching, setIsFetching] = useState(false)
    const [error, setError] = useState(null) 

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                setAuthor(await authorApi.getTopThreeAsync(token))
            } catch (error) {
                setError(error.message)
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [token])

    return { authors, isFetching, error }
}

export function useNames() {
    const { token } = useContext(UserContext) 
    
    const navigate = useNavigate()
    const [authors, setAuthors] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        const fetchData = async () => {
            try {
                setIsFetching(old => !old)
                setAuthors(await authorApi.getAuthorNamesAsync(token))
                setIsFetching(old => !old)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            }
        }

        fetchData()
    }, [token, navigate])

    return { authors, isFetching }
}

export function useSearchAuthors(authors) {
    const [searchTerm, setSearchTerm] = useState('')
    const [filteredAuthors, setFilteredAuthors] = useState([])
    const [showDropdown, setShowDropdown] = useState(false)

    useEffect(() => {
        if (searchTerm === '') {
            setFilteredAuthors([])
        } else {
            const filtered = authors.filter(a =>
                a.name.toLowerCase().includes(searchTerm.toLowerCase())
            )

            setFilteredAuthors(filtered)
        }
    }, [searchTerm, authors])

    const updateSearchTerm = (term) => {
        setSearchTerm(term)
        setShowDropdown(true)
    }

    const selectAuthor = (name) => {
        setSearchTerm(name)
        setShowDropdown(false)
    }

    const showDropdownOnFocus = () => setShowDropdown(true)

    return {
        searchTerm,
        filteredAuthors,
        showDropdown,
        updateSearchTerm,
        selectAuthor,
        showDropdownOnFocus
    }
}

export function useGetDetails(id){
    const { token, isAdmin } = useContext(UserContext)
    
    const navigate = useNavigate()
    const [author, setAuthor] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(old => !old)
                setAuthor(await authorApi.getDetailsAsync(id, token, isAdmin))
                setIsFetching(old => !old)
            } catch(error) {
                navigate(routes.notFound, { state: { message: error.message} })
            }
        }

        fetchData()
    }, [id, token, navigate])

    return { author, isFetching }
}

export function useCreate(){
    const { token } = useContext(UserContext) 

    const navigate = useNavigate()

    const createHandler = async ({ name, penName, imageUrl, gender, biography, nationality, bornAt, diedAt }) => {
        const author = {
            name,
            penName : penName || null,
            imageUrl : imageUrl || null,
            gender,
            biography,
            nationalityId: nationality || null,
            bornAt,
            diedAt
        }

        try {
            const authorId = await authorApi.createAsync(author, token)
            return authorId
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return createHandler
}

export function useEdit(){
    const { token } = useContext(UserContext) 

    const navigate = useNavigate()

    const editHandler = async (authorId, { name, penName, imageUrl, gender, biography, nationality, bornAt, diedAt }) => {
        const author = {
            name,
            penName : penName || null,
            imageUrl : imageUrl || null,
            gender,
            biography,
            nationalityId: nationality,
            bornAt,
            diedAt
        }

        try {
            const isSuccessful = await authorApi.editAsync(authorId, author, token)
            return isSuccessful
        } catch(error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return editHandler
}