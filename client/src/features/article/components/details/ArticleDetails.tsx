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
import { Link } from 'react-router-dom';

import { useDetailsPage } from '@/features/article/hooks/useDetailsPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.jsx';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.jsx';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.jsx';
import { routes } from '@/shared/lib/constants/api.js';

const ArticleDetails: FC = () => {
  const {
    id,
    isAdmin,
    article,
    formattedDate,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
  } = useDetailsPage();

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
                  <MDBCardText className="text-muted">{formattedDate}</MDBCardText>
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12" className="article-image-container">
                  {article.imageUrl}
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
            {isAdmin && (
              <DeleteModal
                showModal={showModal}
                toggleModal={toggleModal}
                deleteHandler={deleteHandler}
              />
            )}
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default ArticleDetails;
