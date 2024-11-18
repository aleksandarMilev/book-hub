import { useContext, useEffect, useState } from 'react'

import * as genreApi from '../api/genreApi'
import { UserContext } from '../contexts/userContext'

export function useGenres() {
    const { token } = useContext(UserContext)
    const [genres, setGenres] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setGenres(await genreApi.getGenresAsync(token))
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
                g => g.name.toLowerCase().includes(searchTerm.toLowerCase()) && !selectedGenres.includes(g)
            )
            
            setFilteredGenres(filtered)
        }
    }, [searchTerm, genres, selectedGenres])

    const updateSearchTerm = (term) => setSearchTerm(term)

    return { searchTerm, filteredGenres, updateSearchTerm }
}