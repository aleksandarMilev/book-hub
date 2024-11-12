import { useContext, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"

import * as bookApi from '../../../api/bookApi'
import * as useBook from '../../../hooks/useBook'
import { routes } from "../../../common/constants/api"
import { UserContext } from "../../../contexts/userContext"

import BookFullInfo from './BookFullInfo'
import AuthorIntroduction from './AuthorIntroduction'
import DefaultSpinner from '../../common/DefaultSpinner'

export default function BookDetails() {
    const { id } = useParams()
    const { userId, token } = useContext(UserContext)
    const { book, isFetching } = useBook.useGetFullInfo(id)
    const [showFullDescription, setShowFullDescription] = useState(false)
    const navigate = useNavigate()

    async function deleteHandler(){
        await bookApi.deleteAsync(id, token)
        navigate(routes.books)
    }

    const bookUserId = book ? book.userId : null
    const isCreator = userId === bookUserId

    const previewTextLength = 200
    const descriptionPreview = book?.longDescription?.slice(0, previewTextLength)

    return (
        !isFetching ? (
          book ? (
            <div className="container mt-5">
                {/* Book and Author Wrapper */}
                <div className="card shadow-lg p-4" style={{ backgroundColor: '#f9f9f9' }}>
                    {/* Book Full Info */}
                    <BookFullInfo
                        book={book}
                        descriptionPreview={descriptionPreview}
                        showFullDescription={showFullDescription}
                        setShowFullDescription={setShowFullDescription}
                        isCreator={isCreator}
                        deleteHandler={deleteHandler}
                        id={id}
                    />

                    {/* Author Introduction */}
                    <AuthorIntroduction author={book.author} />
                </div>
            </div>
          ) : (
            <div className="container mt-5">
              <div className="alert alert-danger text-center" role="alert">
                <h4 className="alert-heading">Oops!</h4>
                <p>The book you are looking for was not found.</p>
              </div>
            </div>
          )
        ) : (
          <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '50vh' }}>
            <DefaultSpinner />
          </div>
        )
    )
}
