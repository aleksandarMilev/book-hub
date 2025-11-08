import './ArticleDetails.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardText,
  MDBCardTitle,
  MDBCol,
  MDBContainer,
  MDBIcon,
  MDBRow,
} from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { FaEdit, FaTrash } from 'react-icons/fa';
import { Link, useParams } from 'react-router-dom';

import * as hooks from '@/features/article/hooks/useArticle';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';
import { routes } from '@/shared/lib/constants/api';
import { formatIsoDate, toIntId } from '@/shared/lib/utils';
import { useAuth } from '@/shared/stores/auth/auth';

const ArticleDetails: FC = () => {
  const { id } = useParams<{ id: string }>();

  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { isAdmin } = useAuth();
  const { data: article, isFetching, error } = hooks.useDetails(parsedId, disable);

  const { showModal, toggleModal, deleteHandler } = hooks.useRemove(
    parsedId,
    disable,
    article?.title,
  );

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !article) {
    return <DefaultSpinner />;
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
                  <MDBCardText className="text-muted">
                    {formatIsoDate(article.createdOn, 'Publish date unavailable')}
                  </MDBCardText>
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12" className="article-image-container">
                  {article.imageUrl ? (
                    <img src={article.imageUrl} alt={article.title} className="article-image" />
                  ) : (
                    <div
                      className="article-image-placeholder"
                      role="img"
                      aria-label="No image available"
                    >
                      No Image Available
                    </div>
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
                    {article.views ?? 0} Views
                  </div>
                </MDBCol>
              </MDBRow>
              {isAdmin && (
                <div className="d-flex gap-2 mt-4">
                  <Link
                    to={`${routes.admin.editArticle}/${id}`}
                    aria-label="Edit article"
                    className="btn btn-warning d-flex align-items-center gap-2"
                  >
                    <FaEdit /> Edit
                  </Link>
                  <button
                    type="button"
                    aria-label="Delete article"
                    className="btn btn-danger d-flex align-items-center gap-2"
                    onClick={deleteHandler}
                  >
                    <FaTrash /> Delete
                  </button>
                </div>
              )}
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
  );
};

export default ArticleDetails;
