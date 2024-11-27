import { useParams } from 'react-router-dom'
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

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './ArticleDetails.css'

export default function ArticleDetails(){
    const { id } = useParams()
    const { article, isFetching } = useArticle.useDetails(id)

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
                        </MDBCardBody>
                    </MDBCard>
                </MDBCol>
            </MDBRow>
        </MDBContainer>
    )
}
