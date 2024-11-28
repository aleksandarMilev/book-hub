import { useContext, useState } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'
import { FaEdit, FaTrash } from 'react-icons/fa'
import { 
    MDBContainer,
    MDBRow,
    MDBCol, 
    MDBCard, 
    MDBCardBody, 
    MDBCardTitle, 
    MDBCardText, 
    MDBIcon 
} from 'mdb-react-ui-kit'

import * as useArticle from '../../../hooks/useArticle'
import * as articleApi from '../../../api/articleApi'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'
import DeleteModal from '../../common/delete-modal/DeleteModal'

import './ArticleDetails.css'

export default function ArticleDetails(){
    const { id } = useParams()
    const navigate = useNavigate()

    const { token } = useContext(UserContext)
 
    const { article, isFetching } = useArticle.useDetails(id)
    const { isAdmin } = useContext(UserContext)

    const [showModal, setShowModal] = useState(false)
    const toggleModal = () => setShowModal(old => !old)

    const deleteHandler = async () => {
        if(showModal){
            try {
                await articleApi.deleteAsync(id, token)
                navigate(routes.home)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            } finally {
                toggleModal()
            }
        } else {
            toggleModal()
        }
    }

    if(isFetching || !article){
        return <DefaultSpinner/>
    }

    return (
        <MDBContainer className="my-5">
            <MDBRow>
                <MDBCol md="8" className="my-col">
                    <MDBCard>
                        <MDBCardBody>
                            <MDBRow>
                                <MDBCol md="12">
                                    <MDBCardTitle className="article-title">{article.title}</MDBCardTitle>
                                    <MDBCardText className="text-muted">Published on {new Date(article.createdOn).toLocaleDateString()}</MDBCardText>
                                </MDBCol>
                            </MDBRow>
                            <MDBRow>
                                <MDBCol md="12" className="article-image-container">
                                    {article.imageUrl ? (
                                        <img src={article.imageUrl} alt={article.title} className="article-image" />
                                    ) : (
                                        <div className="article-image-placeholder">No Image Available</div>
                                    )}
                                </MDBCol>
                            </MDBRow>
                            <MDBRow>
                                <MDBCol md="12">
                                    <MDBCardText className="article-introduction">{article.introduction}</MDBCardText>
                                    <MDBCardText className="article-content">{article.content}</MDBCardText>
                                </MDBCol>
                            </MDBRow>
                            <MDBRow className="mt-4">
                                <MDBCol md="6">
                                    <div className="article-meta">
                                        <MDBIcon fas icon="eye" className="me-2" />
                                        {article.views} Views
                                    </div>
                                </MDBCol>
                            </MDBRow>
                            <div className="d-flex gap-2 mt-4">
                            {isAdmin && (
                                <>
                                    <Link
                                        to={`${routes.admin.editArticle}/${id}`}
                                        className="btn btn-warning d-flex align-items-center gap-2"
                                    >
                                        <FaEdit /> Edit
                                    </Link>
                                    <a href="#" className="btn btn-danger d-flex align-items-center gap-2" onClick={deleteHandler}>
                                        <FaTrash /> Delete
                                    </a>
                                </>
                            )}
                        </div>
                        </MDBCardBody>
                        <DeleteModal
                            showModal={showModal}
                            toggleModal={toggleModal}
                            deleteHandler={deleteHandler}
                        />
                    </MDBCard>
                </MDBCol>
            </MDBRow>
        </MDBContainer>
    )
}
