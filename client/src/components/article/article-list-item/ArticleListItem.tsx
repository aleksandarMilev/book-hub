import type { FC } from 'react';
import { Link } from 'react-router-dom';
import { format } from 'date-fns';
import {
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCardText,
  MDBCardTitle,
} from 'mdb-react-ui-kit';

import { routes } from '../../../common/constants/api';
import './ArticleListItem.css';
import type { ArticleSummary } from '../../../api/article/types/article.type';

const ArticleListItem: FC<ArticleSummary> = ({ id, title, introduction, imageUrl, createdOn }) => {
  return (
    <MDBCard className="mb-4 article-list-item shadow-sm">
      <MDBRow className="g-0">
        <MDBCol md="4">
          {imageUrl ? (
            <MDBCardImage
              src={imageUrl}
              alt={title}
              className="article-item-image"
              onError={(e) => {
                (e.target as HTMLImageElement).src =
                  'https://via.placeholder.com/300x200?text=No+Image';
              }}
            />
          ) : (
            <div className="article-item-image-placeholder d-flex align-items-center justify-content-center text-muted">
              No Image
            </div>
          )}
        </MDBCol>
        <MDBCol md="8">
          <MDBCardBody>
            <MDBCardTitle className="article-item-title">
              <Link to={`${routes.article}/${id}`} className="article-item-link">
                {title}
              </Link>
            </MDBCardTitle>
            <MDBCardText className="article-item-introduction text-muted">
              {introduction}
            </MDBCardText>
            <MDBCardText className="text-muted small">
              Published on {createdOn ? format(new Date(createdOn), 'dd MMM yyyy') : 'Unknown date'}
            </MDBCardText>
          </MDBCardBody>
        </MDBCol>
      </MDBRow>
    </MDBCard>
  );
};

export default ArticleListItem;
