import React from 'react'

import { useSearchGenres } from '../../../../hooks/useBook'

import './GenreSearch.css'

export default function GenreSearch({ genres, loading, formik, selectedGenres, setSelectedGenres }) {
    const {
        searchTerm,
        filteredGenres,
        updateSearchTerm
    } = useSearchGenres(genres, selectedGenres)

    const selectGenre = (genre) => {
        const newSelectedGenres = [...selectedGenres, genre]
        setSelectedGenres(newSelectedGenres) 

        formik.setFieldValue('genres', newSelectedGenres) 
        updateSearchTerm('')  
    }

    const removeGenre = (genre) => {
        const newSelectedGenres = selectedGenres.filter(g => g !== genre)
        setSelectedGenres(newSelectedGenres)

        formik.setFieldValue('genres', newSelectedGenres)
    }

    return (
        <div className="mb-4">
            <h6 className="fw-bold mb-2">{"Genres: * (Select known genres, or 'Other' if unknown)"}</h6>
            <input
                type="text"
                id="genreSearch"
                className="form-control"
                placeholder="Search for genres..."
                value={searchTerm}
                onChange={(e) => updateSearchTerm(e.target.value)} 
            />
            {loading ? (
                <div className="mt-2">Loading...</div>
            ) : (
                <div>
                    {searchTerm && (
                        <ul className="list-group mt-2 genre-list">
                            {filteredGenres.length === 0 ? (
                                <li className="list-group-item">No matches found</li>
                            ) : (
                                filteredGenres.map((g, i) => (
                                    <li
                                        key={i}
                                        className="list-group-item genre-item"
                                        onClick={() => selectGenre(g)} 
                                    >
                                        {g}
                                    </li>
                                ))
                            )}
                        </ul>
                    )}
                    <div className="selected-genres mt-3">
                        {selectedGenres.length > 0 && (
                            <div className="d-flex flex-wrap">
                                {selectedGenres.map((g, i) => (
                                    <span key={i} className="badge badge-secondary m-1">
                                        {g}
                                        <span
                                            className="badge-close"
                                            onClick={() => removeGenre(g)}
                                        >
                                            &times;
                                        </span>
                                    </span>
                                ))}
                            </div>
                        )}
                    </div>
                </div>
            )}
            {formik.touched.genres && formik.errors.genres && (
                <div className="text-danger">{formik.errors.genres}</div>
            )}
        </div>
    )
}
