import type { FC } from 'react';
import { Link } from 'react-router-dom';
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
import { formatIsoDate } from '../../../common/functions/utils';
import type { ArticleSummary } from '../../../api/article/types/article';

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
            <div
              className="article-item-image-placeholder d-flex align-items-center justify-content-center text-muted"
              role="img"
              aria-label="No image available"
            >
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
              Published on {formatIsoDate(createdOn, 'Publish date unavailable')}
            </MDBCardText>
          </MDBCardBody>
        </MDBCol>
      </MDBRow>
    </MDBCard>
  );
};

export default ArticleListItem;
