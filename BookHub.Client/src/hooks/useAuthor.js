import { useContext, useEffect, useState } from 'react'

import * as authorApi from '../api/authorApi'
import { UserContext } from '../contexts/userContext'

export function useNationalities() {
    const { token } = useContext(UserContext) 
    const [nationalities, setNationalities] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setNationalities(await authorApi.getNationalitiesAsync(token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [])

    return { nationalities, isFetching }
}

export function useSearchNationalities(nationalities) {
    const [searchTerm, setSearchTerm] = useState('')
    const [filteredNationalities, setFilteredNationalities] = useState([])
    const [showDropdown, setShowDropdown] = useState(false)

    useEffect(() => {
        if (searchTerm === '') {
            setFilteredNationalities([])
        } else {
            const filtered = nationalities.filter(n =>
                n.toLowerCase().includes(searchTerm.toLowerCase())
            )

            setFilteredNationalities(filtered)
        }
    }, [searchTerm, nationalities])

    const updateSearchTerm = (term) => {
        setSearchTerm(term)
        setShowDropdown(true)
    }

    const selectNationality = (nationality) => {
        setSearchTerm(nationality)
        setShowDropdown(false)
    }

    const showDropdownOnFocus = () => setShowDropdown(true)

    return {
        searchTerm,
        filteredNationalities,
        showDropdown,
        updateSearchTerm,
        selectNationality,
        showDropdownOnFocus
    }
}

export function useGetDetails(id){
    const { token } = useContext(UserContext)
    const [author, setAuthor] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setAuthor(await authorApi.getDetailsAsync(id, token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [id])

    return { author, isFetching }
}

export function useGetTopThree(){
    const { token } = useContext(UserContext)
    const [authors, setAuthor] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setAuthor(await authorApi.getTopThreeAsync(token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [])

    return { authors, isFetching }
}

export function useCreate(){
    const { token } = useContext(UserContext) 

    const createHandler = async ({ name, penName, imageUrl, gender, biography, nationality, bornAt, diedAt }) => {
        const author = {
            name,
            penName : penName || null,
            imageUrl : imageUrl || null,
            gender,
            biography,
            nationality,
            bornAt,
            diedAt
        }

        try {
            const authorId = await authorApi.createAsync(author, token)
            return authorId
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return createHandler
}

export function useEdit(){
    const { token } = useContext(UserContext) 

    const editHandler = async (authorId, { name, penName, imageUrl, gender, biography, nationality, bornAt, diedAt }) => {
        const author = {
            name,
            penName : penName || null,
            imageUrl : imageUrl || null,
            gender,
            biography,
            nationality,
            bornAt,
            diedAt
        }

        try {
            await authorApi.editAsync(authorId, author, token)
        } catch (error) {
            throw new Error(error.message)
        }
    }

    return editHandler
}

export function useNames() {
    const { token } = useContext(UserContext) 
    const [authors, setAuthors] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setAuthors(await authorApi.getAuthorNamesAsync(token))
            setIsFetching(old => !old)
        }

        fetchData()
    }, [])

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
                a.toLowerCase().includes(searchTerm.toLowerCase())
            )

            setFilteredAuthors(filtered)
        }
    }, [searchTerm, authors])

    const updateSearchTerm = (term) => {
        setSearchTerm(term)
        setShowDropdown(true)
    }

    const selectAuthor = (author) => {
        setSearchTerm(author)
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