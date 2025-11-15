import './ArticleListItem.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCardText,
  MDBCardTitle,
  MDBCol,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { Link } from 'react-router-dom';

import type { ArticlesSearchResult } from '@/features/search/types/search.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate } from '@/shared/lib/utils.js';

const ArticleListItem: FC<ArticlesSearchResult> = ({
  id,
  title,
  introduction,
  imageUrl,
  createdOn,
}) => {
  return (
    <MDBCard className="mb-4 article-list-item shadow-sm">
      <MDBRow className="g-0">
        <MDBCol md="4">
          {imageUrl ? (
            <MDBCardImage
              src={imageUrl}
              alt={title}
              className="article-item-image"
              onError={(e: Event) => {
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
