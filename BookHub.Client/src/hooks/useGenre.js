import { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'

import * as genreApi from '../api/genreApi'
import { errors } from '../common/constants/messages'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useGenres() {
    const navigate = useNavigate()
    const [genres, setGenres] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    const { token } = useContext(UserContext)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(old => !old)
                setGenres(await genreApi.getGenresAsync(token))
                setIsFetching(old => !old)
            } catch {
                navigate(routes.badRequest, { state: { message: errors.genre.namesBadRequest } })
            }
        }

        fetchData()
    }, [token, navigate])

    return { genres, isFetching }
}

export function useSearchGenres(genres, selectedGenres) {
    const [searchTerm, setSearchTerm] = useState('')
    const [filteredGenres, setFilteredGenres] = useState([])

    useEffect(() => {
        if (searchTerm === '') {
            setFilteredGenres([])
        } else {
            const filtered = genres.filter(g =>
                g.name.toLowerCase().includes(searchTerm.toLowerCase()) && !selectedGenres.includes(g)
            )
            
            setFilteredGenres(filtered)
        }
    }, [searchTerm, genres, selectedGenres])

    const updateSearchTerm = (term) => setSearchTerm(term)

    return { searchTerm, filteredGenres, updateSearchTerm }
}