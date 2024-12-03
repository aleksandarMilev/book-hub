import { Link } from 'react-router-dom'
import { format } from 'date-fns'
import { 
    MDBRow,
    MDBCol, 
    MDBCard, 
    MDBCardBody, 
    MDBCardImage, 
    MDBCardText, 
    MDBCardTitle 
} from 'mdb-react-ui-kit'

import { routes } from '../../../common/constants/api'

import './ArticleListItem.css'

export default function ArticleListItem({ id, title, introduction, imageUrl, createdOn }) {
    return (
        <MDBCard className="mb-4 article-list-item">
            <MDBRow className="g-0">
                <MDBCol md="4">
                    {imageUrl ? (
                        <MDBCardImage
                            src={imageUrl}
                            alt={title}
                            className="article-item-image"
                        />
                    ) : (
                        <div className="article-item-image-placeholder">No Image</div>
                    )}
                </MDBCol>
                <MDBCol md="8">
                    <MDBCardBody>
                        <MDBCardTitle className="article-item-title">
                            <Link to={routes.article + `/${id}`} className="article-item-link">
                                {title}
                            </Link>
                        </MDBCardTitle>
                        <MDBCardText className="article-item-introduction">{introduction}</MDBCardText>
                        <MDBCardText className="text-muted">
                            Published on {format(new Date(createdOn), 'dd MMM yyyy')}
                        </MDBCardText>
                    </MDBCardBody>
                </MDBCol>
            </MDBRow>
        </MDBCard>
    )
}