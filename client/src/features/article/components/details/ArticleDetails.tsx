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
import type { FC } from 'react';
import { FaEdit, FaTrash } from 'react-icons/fa';
import ReactMarkdown from 'react-markdown';
import { Link } from 'react-router-dom';
import remarkGfm from 'remark-gfm';

import { useDetailsPage } from '@/features/article/hooks/useDetailsPage.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect.js';
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
    <MDBContainer className="article-details-page my-5">
      <MDBRow className="justify-content-center">
        <MDBCol lg="9" className="article-details-col">
          <MDBCard className="article-details-card">
            <MDBCardBody className="article-details-body">
              <MDBRow>
                <MDBCol md="12">
                  <MDBCardTitle className="article-details-title">{article.title}</MDBCardTitle>
                  <MDBCardText className="article-details-meta">{formattedDate}</MDBCardText>
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12">
                  <div className="article-details-image-wrapper">
                    <img
                      src={article.imageUrl}
                      alt={article.title}
                      className="article-details-image"
                    />
                  </div>
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12">
                  <MDBCardText className="article-details-intro">
                    {article.introduction}
                  </MDBCardText>
                  <div className="article-details-content">
                    <ReactMarkdown remarkPlugins={[remarkGfm]}>{article.content}</ReactMarkdown>
                  </div>
                </MDBCol>
              </MDBRow>
              <MDBRow className="article-details-bottom-meta">
                <MDBCol md="6">
                  <div className="article-details-meta">
                    <MDBIcon fas icon="eye" className="me-2" />
                    {article.views ?? 0} Views
                  </div>
                </MDBCol>
              </MDBRow>
              {isAdmin && (
                <div className="d-flex gap-2 article-details-actions">
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
