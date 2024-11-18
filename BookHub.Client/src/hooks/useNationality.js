import { useContext, useEffect, useState } from 'react'

import * as nationalityApi from '../api/nationalityApi'
import { UserContext } from '../contexts/userContext'

export function useNationalities() {
    const { token } = useContext(UserContext) 
    const [nationalities, setNationalities] = useState([])
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            setIsFetching(old => !old)
            setNationalities(await nationalityApi.getNationalitiesAsync(token))
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
                n.name.toLowerCase().includes(searchTerm.toLowerCase())
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