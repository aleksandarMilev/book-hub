import { useContext, useState } from 'react'
import { Link, useParams, useNavigate } from 'react-router-dom'
import { format } from 'date-fns'
import { FaEdit, FaTrashAlt, FaTrash } from 'react-icons/fa' 
import { 
    MDBCol, 
    MDBContainer,
    MDBRow, 
    MDBCard, 
    MDBCardText, 
    MDBCardBody, 
    MDBCardImage, 
    MDBTypography, 
    MDBBtn
} from 'mdb-react-ui-kit' 

import * as authorApi from '../../../api/authorApi'
import * as useAuthor from '../../../hooks/useAuthor'
import renderStars from '../../../common/functions/renderStars'
import { errors } from '../../../common/constants/messages'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import BookListItem from '../../book/book-list-item/BooksListItem'
import DeleteModal from '../../common/delete-modal/DeleteModal'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './AuthorDetails.css'

export default function AuthorDetails() {
    const { id } = useParams()
    const navigate = useNavigate()
    const [showModal, setShowModal] = useState(false)
    
    const { userId, token, isAdmin } = useContext(UserContext)

    const { author, isFetching } = useAuthor.useGetDetails(id)

    const approveHandler = async () => {
        try {
            await authorApi.approveAsync(id, token)
            navigate(routes.author + `/${id}`)
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    const rejectHandler = async () => {
        try {
            await authorApi.rejectAsync(id, token)
            navigate(routes.home)
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    const toggleModal = () => setShowModal(prev => !prev)

    const deleteHandler = async() => {
        if (showModal) {
            const success = await authorApi.deleteAsync(id, token)
            
            if(success){
                navigate(routes.book)
            } else {
                navigate(routes.badRequest, { state: { message: errors.author.delete } })
            }
        } else {
            toggleModal()  
        }
    }

    if (isFetching || !author) {
        return <DefaultSpinner />
    } 

    const isCreator = author?.creatorId === userId

    return (
        <div className="author-details-wrapper">
            <MDBContainer className="py-5 h-100">
                <MDBRow className="justify-content-center align-items-center h-100">
                    <MDBCol lg="10">
                        <MDBCard className="author-card">
                            <MDBCardBody>
                                <div className="author-header">
                                    <MDBCardImage
                                        src={author.imageUrl}
                                        alt={`${author.name}'s image`}
                                        className="author-image"
                                        fluid
                                    />
                                    <div>
                                        <MDBTypography tag="h2" className="author-name">
                                            {author.name}
                                        </MDBTypography>
                                        <MDBCardText className="author-nationality">{author.nationality.name}</MDBCardText>
                                        <MDBCardText className="author-penname">
                                            {author.penName ? `Pen Name: ${author.penName}` : "No Pen Name"}
                                        </MDBCardText>
                                    </div>
                                </div>
                                {isCreator && (
                                    <div className="author-actions">
                                        <Link to={`${routes.editAuthor}/${author.id}`} className="me-2">
                                            <MDBBtn outline color="warning" size="sm">
                                                <FaEdit className="me-1" /> Edit
                                            </MDBBtn>
                                        </Link>
                                        <MDBBtn
                                            outline
                                            color="danger"
                                            size="sm"
                                            onClick={toggleModal}
                                        >
                                            <FaTrashAlt className="me-1" /> Delete
                                        </MDBBtn>
                                    </div>
                                )}
                                {(isAdmin && !author.isApproved) && (
                                    <div className="author-actions">
                                        <a href="#" className="btn btn-success d-flex align-items-center gap-2" onClick={approveHandler}>
                                            <FaTrash /> Approve
                                        </a>
                                        <a href="#" className="btn btn-danger d-flex align-items-center gap-2" onClick={rejectHandler}>
                                            <FaTrash /> Reject
                                        </a>
                                    </div>
                                )}
                                <section className="author-about">
                                    <MDBTypography tag="h4" className="section-title">About</MDBTypography>
                                    <MDBCardText className="author-biography">{author.biography}</MDBCardText>
                                    <MDBCardText className="author-birthdate">
                                        <strong>Born:</strong> {author.bornAt ? format(new Date(author.bornAt), 'MMM dd, yyyy') : "Unknown"}
                                        {author.bornAt && !author.diedAt ? ` (${new Date().getFullYear() - new Date(author.bornAt).getFullYear()} years old)` : ""}
                                    </MDBCardText>
                                    <MDBCardText className="author-deathdate">
                                        {author.diedAt && (
                                            <>
                                                <strong>Died: </strong>
                                                {format(new Date(author.diedAt), 'MMM dd, yyyy')}
                                                {author.bornAt && ` (${new Date(author.diedAt).getFullYear() - new Date(author.bornAt).getFullYear()} years old)`}
                                            </>
                                        )}
                                    </MDBCardText>
                                </section>
                                <section className="author-statistics">
                                    <MDBTypography tag="h4" className="section-title">Statistics</MDBTypography>
                                    <MDBRow className="text-center mt-3">
                                        <MDBCol className="d-flex flex-column align-items-center">
                                            <MDBCardText className="author-rating">{renderStars(author.averageRating)}</MDBCardText>
                                            <MDBCardText className="author-rating-text">Average Rating</MDBCardText>
                                        </MDBCol>
                                        <MDBCol className="d-flex flex-column align-items-center">
                                            <MDBCardText className="author-books-count">{author.booksCount}</MDBCardText>
                                            <MDBCardText className="author-books-text">Books Published</MDBCardText>
                                        </MDBCol>
                                    </MDBRow>
                                </section>
                                <section className="author-top-books">
                                    <MDBTypography tag="h4" className="section-title">Top Books</MDBTypography>
                                    {author.topBooks && author.topBooks.length > 0 ? (
                                        author.topBooks.map(b => (
                                            <BookListItem key={b.id} {...b} /> 
                                        ))
                                    ) : (
                                        <MDBCardText>No books available for this author.</MDBCardText>
                                    )}
                                </section>
                            </MDBCardBody>
                        </MDBCard>
                    </MDBCol>
                </MDBRow>
            </MDBContainer>
            <DeleteModal
                showModal={showModal}
                toggleModal={toggleModal}
                deleteHandler={deleteHandler}
            />
        </div>
    )
}
