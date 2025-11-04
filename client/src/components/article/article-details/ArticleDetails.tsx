import { type FC, useContext } from 'react';
import { useParams, Link } from 'react-router-dom';
import { FaEdit, FaTrash } from 'react-icons/fa';
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBIcon,
} from 'mdb-react-ui-kit';

import * as hooks from '../../../hooks/useArticle';
import { routes } from '../../../common/constants/api';

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import DeleteModal from '../../common/delete-modal/DeleteModal';
import { UserContext } from '../../../contexts/user/userContext';
import { formatIsoDate, parseId } from '../../../common/functions/utils';

import './ArticleDetails.css';

const ArticleDetails: FC = () => {
  const { id } = useParams<{ id: string }>();
  let parsedId: number | null = null;

  try {
    parsedId = parseId(id);
  } catch {}

  if (parsedId == null) {
    return <div>Invalid article id.</div>;
  }

  const { isAdmin } = useContext(UserContext);
  const { article, isFetching, error } = hooks.useDetails(parsedId);
  const { showModal, toggleModal, deleteHandler } = hooks.useRemove(parsedId, article?.title);

  if (error) {
    return <div className="alert alert-danger">{error}</div>;
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
