import { useContext, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"

import * as bookApi from '../../../api/bookApi'
import * as useBook from '../../../hooks/useBook'
import { errors } from "../../../common/constants/messages"
import { routes } from "../../../common/constants/api"
import { UserContext } from "../../../contexts/userContext"

import BookFullInfo from './book-full-info/BookFullInfo'
import AuthorIntroduction from './author-introduction/AuthorIntroduction'
import ReviewForm from './review-form/ReviewForm'
import DeleteModal from '../../common/delete-modal/DeleteModal'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './BookDetails.css'

export default function BookDetails() {
    const { id } = useParams()
    const navigate = useNavigate()
    const [showModal, setShowModal] = useState(false) 
    const [showFullDescription, setShowFullDescription] = useState(false)

    const { userId, token } = useContext(UserContext)
    const { book, isFetching } = useBook.useGetFullInfo(id)

    const toggleModal = () => setShowModal(prev => !prev)

    async function deleteHandler() {
        if (showModal) {
            const success = await bookApi.deleteAsync(id, token)
            
            if(success){
                navigate(routes.books)
            } else {
                navigate(routes.badRequest, { state: { message: errors.book.delete } })
            }
        } else {
            toggleModal()  
        }
    }

    if(isFetching || !book){
        return(
            <div className="spinner-container d-flex justify-content-center align-items-center">
                <DefaultSpinner />
            </div>
        )
    }

    const creatorId = book ? book.creatorId : null
    const isCreator = userId === creatorId

    const previewTextLength = 200
    const descriptionPreview = book?.longDescription?.slice(0, previewTextLength)

    return (
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
                    <ReviewForm />
                </div>
                    <DeleteModal 
                        showModal={showModal} 
                        toggleModal={toggleModal} 
                        deleteHandler={deleteHandler} 
                    />
            </div>
        )   
}
