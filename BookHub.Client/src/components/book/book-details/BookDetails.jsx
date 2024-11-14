import { useContext, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { FaExclamationTriangle, FaTrashAlt } from "react-icons/fa"

import * as bookApi from '../../../api/bookApi'
import * as useBook from '../../../hooks/useBook'
import { routes } from "../../../common/constants/api"
import { UserContext } from "../../../contexts/userContext"

import BookFullInfo from './book-full-info/BookFullInfo'
import AuthorIntroduction from './author-introduction/AuthorIntroduction'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './BookDetails.css'

export default function BookDetails() {
    const { id } = useParams()
    const { userId, token } = useContext(UserContext)
    const { book, isFetching } = useBook.useGetFullInfo(id)
    const [showFullDescription, setShowFullDescription] = useState(false)
    const [showModal, setShowModal] = useState(false) 
    const navigate = useNavigate()

    const toggleModal = () => setShowModal(prev => !prev)

    async function deleteHandler() {
        if (showModal) {
            await bookApi.deleteAsync(id, token)
            navigate(routes.books)
        } else {
            toggleModal()  
        }
    }

    const creatorId = book ? book.creatorId : null
    const isCreator = userId === creatorId

    const previewTextLength = 200
    const descriptionPreview = book?.longDescription?.slice(0, previewTextLength)

    return (
        !isFetching ? (
            book ? (
                <div className="book-details-container mt-5">
                    <div className="book-details-card shadow-lg p-4">
                        <BookFullInfo
                            book={book}
                            descriptionPreview={descriptionPreview}
                            showFullDescription={showFullDescription}
                            setShowFullDescription={setShowFullDescription}
                            isCreator={isCreator}
                            deleteHandler={deleteHandler}
                            id={id}
                        />
                        <AuthorIntroduction author={book.author} />
                    </div>
                    <div className={`modal fade ${showModal ? 'show d-block' : ''}`} tabIndex="-1" aria-labelledby="deleteModalLabel" aria-hidden={!showModal}>
                        <div className="modal-dialog modal-dialog-centered">
                            <div className="modal-content">
                                <div className="modal-header bg-warning text-white">
                                    <h5 className="modal-title" id="deleteModalLabel">
                                        <FaExclamationTriangle className="me-2" /> Confirm Deletion
                                    </h5>
                                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" onClick={toggleModal}></button>
                                </div>
                                <div className="modal-body">
                                    <p className="text-center">Are you sure you want to delete this book? This action cannot be undone.</p>
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn btn-secondary" data-bs-dismiss="modal" onClick={toggleModal}>Cancel</button>
                                    <button type="button" className="btn btn-danger" onClick={deleteHandler}>
                                        <FaTrashAlt className="me-2" /> Delete
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            ) : (
                <div className="book-not-found mt-5">
                    <div className="alert alert-danger text-center" role="alert">
                        <h4 className="alert-heading">Oops!</h4>
                        <p>The book you are looking for was not found.</p>
                    </div>
                </div>
            )
        ) : (
            <div className="spinner-container d-flex justify-content-center align-items-center">
                <DefaultSpinner />
            </div>
        )
    )
}
