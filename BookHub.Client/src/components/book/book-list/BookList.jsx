import { useState } from 'react' 
import { FaSearch } from 'react-icons/fa'

import * as useSearch from '../../../hooks/useSearch'

import BookListItem from '../book-list-item/BooksListItem'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import image from '../../../assets/images/no-books-found.png'

import './BookList.css'

export default function BookList() {
    const [searchTerm, setSearchTerm] = useState('')
    const { books, isFetching } = useSearch.useBooks(searchTerm)

    const handleSearchChange = (e) => {
        setSearchTerm(e.target.value)
    }

    return (
        <div className="container mt-5 mb-5">
            <div className="row mb-4">
                <div className="col-md-10 mx-auto d-flex">
                    <div className="search-bar-container d-flex">
                        <input
                            type="text"
                            className="form-control search-input"
                            placeholder="Search books..."
                            value={searchTerm}
                            onChange={handleSearchChange}
                        />
                        <button
                            className="btn btn-light search-btn"
                            disabled={isFetching} 
                        >
                            <FaSearch size={20} /> 
                        </button>
                    </div>
                </div>
            </div>
            <div className="d-flex justify-content-center row">
                <div className="col-md-10">
                    {isFetching
                        ? <DefaultSpinner /> 
                        : books.length > 0
                            ? books.map(b => (
                                <BookListItem key={b.id} {...b} /> 
                            ))
                            : (
                                <div className="d-flex flex-column align-items-center justify-content-center mt-5">
                                    <img 
                                        src={image} 
                                        alt="No books found" 
                                        className="mb-4" 
                                        style={{ maxWidth: '200px', opacity: 0.7 }}
                                    />
                                    <h5 className="text-muted">We couldn't find any books</h5>
                                    <p className="text-muted text-center" style={{ maxWidth: '400px' }}>
                                        Try adjusting your search terms or exploring our collection for more options.
                                    </p>
                                </div>
                            )
                    }
                </div>
            </div>
        </div>
    )
}
