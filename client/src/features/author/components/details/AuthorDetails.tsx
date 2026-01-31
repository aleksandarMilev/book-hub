import './AuthorDetails.css';

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
import { useTranslation } from 'react-i18next';
import { FaEdit, FaTrash } from 'react-icons/fa';
import ReactMarkdown from 'react-markdown';
import { Link } from 'react-router-dom';
import remarkGfm from 'remark-gfm';

import ApproveRejectButtons from '@/features/author/components/details/approve-reject-buttons/ApproveRejectButtons';
import { useDetailsPage } from '@/features/author/hooks/useDetailsPage';
import { getNationalityName } from '@/features/author/types/author';
import { formatBiography } from '@/features/author/utils/utils';
import BookListItem from '@/features/book/components/list-item/BookListItem';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { routes } from '@/shared/lib/constants/api';
import { calculateAge, formatIsoDate, getImageUrl } from '@/shared/lib/utils/utils';

const AuthorDetails: FC = () => {
  const { t } = useTranslation('authors');
  const {
    id,
    token,
    isAdmin,
    userId,
    author,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
    navigate,
    showMessage,
  } = useDetailsPage();

  if (error) {
    return <ErrorRedirect error={error} />;
  }

  if (isFetching || !author) {
    return <DefaultSpinner />;
  }

  const isCreator = author.creatorId === userId;

  const bornText = author.bornAt
    ? formatIsoDate(author.bornAt, t('details.birth.unknown'))
    : t('details.birth.unknown');

  const diedText = author.diedAt
    ? formatIsoDate(author.diedAt, t('details.death.unknown'))
    : t('details.death.unknown');

  const booksCount = author.booksCount ?? 0;
  const booksText =
    booksCount > 0
      ? t('details.booksPublished', { count: booksCount })
      : t('details.booksPublishedNone');

  return (
    <MDBContainer className="author-details-page my-5">
      <MDBRow>
        <MDBCol md="8" className="author-details-col">
          <MDBCard className="author-details-card">
            <MDBCardBody className="author-details-body">
              <nav aria-label="breadcrumb" className="author-details-breadcrumb">
                <ol className="breadcrumb mb-2">
                  <li className="breadcrumb-item">
                    <Link to={routes.home}>{t('details.breadcrumb.home')}</Link>
                  </li>
                  <li className="breadcrumb-item">
                    <Link to={routes.author}>{t('details.breadcrumb.list')}</Link>
                  </li>
                  <li className="breadcrumb-item active" aria-current="page">
                    {author.name}
                  </li>
                </ol>
              </nav>
              <MDBRow className="author-header-row align-items-center">
                <MDBCol
                  md="4"
                  className="d-flex justify-content-center justify-content-md-start mb-3 mb-md-0"
                >
                  <div className="author-image-container">
                    {author.imagePath ? (
                      <img
                        src={getImageUrl(author.imagePath, 'authors')}
                        alt={author.name}
                        className="author-details-image"
                      />
                    ) : (
                      <div className="author-details-image-placeholder">
                        {t('details.image.noImage')}
                      </div>
                    )}
                  </div>
                </MDBCol>
                <MDBCol md="8">
                  <MDBCardTitle className="author-title">{author.name}</MDBCardTitle>
                  <MDBCardText className="author-subtitle">
                    <strong>{t('details.nationality.label')}</strong>{' '}
                    {getNationalityName(author.nationality)}
                    {author.penName && (
                      <>
                        <br />
                        {t('details.penName', { penName: author.penName })}
                      </>
                    )}
                  </MDBCardText>
                  <MDBCardText className="text-muted mt-2">
                    <strong>{t('details.sex.label')}</strong> {author.gender}
                  </MDBCardText>
                  <div className="author-header-meta">
                    <div className="author-meta-pill">
                      <MDBIcon fas icon="star" className="me-2" />
                      <span className="me-1">{t('details.rating.label')}</span>
                      <RenderStars rating={author.averageRating ?? 0} />
                    </div>
                    <div className="author-meta-pill">
                      <MDBIcon fas icon="book" className="me-2" />
                      {booksText}
                    </div>
                  </div>
                </MDBCol>
              </MDBRow>
              <MDBRow>
                <MDBCol md="12">
                  <div className="author-details-content">
                    <ReactMarkdown remarkPlugins={[remarkGfm]}>
                      {formatBiography(author.biography)}
                    </ReactMarkdown>
                  </div>
                  <MDBCardText className="text-muted mt-3">
                    <strong>{t('details.birth.label')}</strong> {bornText}
                    {author.bornAt && !author.diedAt
                      ? t('details.birth.ageSuffix', {
                          age: calculateAge(author.bornAt),
                        })
                      : ''}
                  </MDBCardText>
                  {author.diedAt && (
                    <MDBCardText className="text-muted">
                      <strong>{t('details.death.label')}</strong> {diedText}
                      {author.bornAt &&
                        t('details.death.ageSuffix', {
                          age: calculateAge(author.bornAt, author.diedAt),
                        })}
                    </MDBCardText>
                  )}
                </MDBCol>
              </MDBRow>
              {(isCreator || isAdmin) && author.isApproved && (
                <div className="author-details-actions d-flex gap-2 mt-4">
                  <Link
                    to={`${routes.editAuthor}/${id}`}
                    className="btn btn-warning d-flex align-items-center gap-2"
                    aria-label={t('details.actions.editLabel')}
                  >
                    <FaEdit /> {t('details.actions.edit')}
                  </Link>
                  <button
                    type="button"
                    className="btn btn-danger d-flex align-items-center gap-2"
                    onClick={toggleModal}
                    aria-label={t('details.actions.deleteLabel')}
                  >
                    <FaTrash /> {t('details.actions.delete')}
                  </button>
                </div>
              )}
              {isAdmin && !author.isApproved && (
                <div className="mt-4">
                  <ApproveRejectButtons
                    authorId={author.id}
                    authorName={author.name}
                    initialIsApproved={author.isApproved ?? false}
                    token={token}
                    onSuccess={(message, success) => showMessage(message, !!success)}
                  />
                </div>
              )}
              <MDBRow className="mt-4">
                <MDBCol md="12">
                  <MDBCardTitle className="author-section-title">
                    {t('details.topBooks.title')}
                  </MDBCardTitle>
                  {author.topBooks && author.topBooks.length > 0 ? (
                    <>
                      {author.topBooks.map((b) => (
                        <BookListItem key={b.id} {...b} />
                      ))}
                      <div className="d-flex justify-content-center mt-3">
                        <button
                          type="button"
                          className="btn btn-primary"
                          onClick={() =>
                            navigate(routes.book, {
                              state: { authorId: author.id, authorName: author.name },
                            })
                          }
                        >
                          {t('details.topBooks.viewAll', { name: author.name })}
                        </button>
                      </div>
                    </>
                  ) : (
                    <MDBCardText>{t('details.topBooks.none')}</MDBCardText>
                  )}
                </MDBCol>
              </MDBRow>
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

export default AuthorDetails;


