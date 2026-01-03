import './ProfileDetails.css';

import {
  faBirthdayCake,
  faBook,
  faBookReader,
  faChartBar,
  faComment,
  faEdit,
  faGlobe,
  faLock,
  faTrashAlt,
  faUser,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import BookListItem from '@/features/book/components/list-item/BookListItem.js';
import { useInviteToChat } from '@/features/chat/hooks/useCrud.js';
import { useDetails } from '@/features/profile/hooks/useCrud.js';
import type { PrivateProfile, Profile } from '@/features/profile/types/profile.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate, getImageUrl, IsError } from '@/shared/lib/utils/utils.js';
import { useMessage } from '@/shared/stores/message/message.js';

const isFullProfile = (profile: Profile | PrivateProfile | null | undefined): profile is Profile =>
  !!profile && 'createdBooksCount' in profile;

const ProfileDetails = () => {
  const {
    profile,
    userId,
    isAdmin,
    canSeePrivate,
    showModal,
    toggleModal,
    deleteHandler,
    book,
    readingLoading,
    chatButtons,
    chatLoading,
    chatError,
    onNavigateRead,
    onNavigateToRead,
    onNavigateCurrentlyReading,
    profileLoading,
    refetchChats,
  } = useDetails();

  const { showMessage } = useMessage();
  const inviteToChat = useInviteToChat(refetchChats);
  const { t } = useTranslation('profiles');

  if (profileLoading) {
    return <DefaultSpinner />;
  }

  const full = isFullProfile(profile) ? profile : null;

  return (
    <div className="profile-details container-fluid">
      <div className="main-body">
        <div className="row align-items-center justify-content-center">
          <div className="col-lg-6">
            <div className="card">
              <div className="card-body">
                {profile && isAdmin && (
                  <button type="button" className="btn btn-outline-danger" onClick={toggleModal}>
                    <FontAwesomeIcon icon={faTrashAlt} className="me-2" />
                    {t('details.buttons.delete')}
                  </button>
                )}
                <div className="d-flex flex-column align-items-center text-center">
                  <img
                    src={getImageUrl(profile?.imagePath ?? '', 'profiles')}
                    alt="Profile"
                    className="rounded-circle p-1 bg-primary"
                    width="110"
                  />
                  <div className="mt-3">
                    <h4>
                      {profile
                        ? `${profile.firstName} ${profile.lastName}`
                        : t('details.labels.nameFallback')}
                    </h4>
                  </div>
                </div>
                {canSeePrivate ? (
                  <>
                    <hr className="my-4" />
                    <ul className="list-group list-group-flush">
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faBirthdayCake} className="me-2 text-danger" />
                          {t('details.labels.dateOfBirth')}
                        </h6>
                        <span className="text-secondary">
                          {full
                            ? full.dateOfBirth
                              ? formatIsoDate(full.dateOfBirth)
                              : t('details.labels.dateOfBirthFallback')
                            : t('details.labels.dateOfBirthFallback')}
                        </span>
                      </li>
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faGlobe} className="me-2 text-info" />
                          {t('details.labels.website')}
                        </h6>
                        <span className="text-secondary">
                          {full && full.socialMediaUrl ? (
                            <a href={full.socialMediaUrl} target="_blank" rel="noopener noreferrer">
                              {full.socialMediaUrl}
                            </a>
                          ) : (
                            t('details.labels.websiteFallback')
                          )}
                        </span>
                      </li>
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faLock} className="me-2 text-warning" />
                          {t('details.labels.accountPrivacy')}
                        </h6>
                        <span className="text-secondary">
                          {profile?.isPrivate
                            ? t('details.labels.privacyPrivate')
                            : t('details.labels.privacyPublic')}
                        </span>
                      </li>
                    </ul>
                    <hr className="my-4" />
                    <div className="list-group-item d-flex flex-column align-items-start">
                      <h6 className="mb-0">
                        <FontAwesomeIcon icon={faUser} className="me-2 text-success" />
                        {t('details.labels.biography')}
                      </h6>
                      <span className="text-secondary mt-2">
                        {full && full.biography ? (
                          <p className="bio-text">{full.biography}</p>
                        ) : (
                          t('details.labels.biographyFallback')
                        )}
                      </span>
                    </div>
                    <hr className="my-4" />
                    <div className="statistics-section">
                      <h6 className="mb-0">
                        <FontAwesomeIcon icon={faChartBar} className="me-2 text-info" />
                        {t('details.stats.title')}
                      </h6>
                      <div className="statistics-items mt-3">
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBook} className="me-2 text-info" />
                            {t('details.stats.booksCreated')}
                          </span>
                          <span className="stat-value">
                            {full ? (full.createdBooksCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faUser} className="me-2 text-success" />
                            {t('details.stats.authorsCreated')}
                          </span>
                          <span className="stat-value">
                            {full ? (full.createdAuthorsCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faComment} className="me-2 text-warning" />
                            {t('details.stats.reviewsWritten')}
                          </span>
                          <span className="stat-value">{full ? (full.reviewsCount ?? 0) : 0}</span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBookReader} className="me-2 text-warning" />
                            {t('details.stats.currentlyReading')}
                          </span>
                          <span className="stat-value">
                            {full ? (full.currentlyReadingBooksCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBookReader} className="me-2 text-warning" />
                            {t('details.stats.wantToRead')}
                          </span>
                          <span className="stat-value">
                            {full ? (full.toReadBooksCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBookReader} className="me-2 text-warning" />
                            {t('details.stats.read')}
                          </span>
                          <span className="stat-value">
                            {full ? (full.readBooksCount ?? 0) : 0}
                          </span>
                        </div>
                      </div>
                    </div>
                    <hr className="my-4" />
                    <div className="d-flex justify-content-around mt-3">
                      {profile && userId !== profile.id && !profile.isPrivate && (
                        <section className="chat-section">
                          <h3 className="chat-section-heading">{t('details.chat.title')}</h3>
                          <div className="chat-buttons-container">
                            {chatError ? (
                              <p className="chat-buttons-error">{chatError}</p>
                            ) : (
                              !chatLoading &&
                              chatButtons?.map((c) => (
                                <button
                                  key={c.id}
                                  className="chat-button"
                                  onClick={() =>
                                    inviteToChat(c.id, profile.id, profile.firstName, c.name)
                                  }
                                >
                                  {c.name}
                                </button>
                              ))
                            )}
                          </div>
                        </section>
                      )}
                      {profile && userId === profile.id && (
                        <>
                          <Link to={routes.editProfile} className="btn btn-outline-primary">
                            <FontAwesomeIcon icon={faEdit} className="me-2" />
                            {t('details.buttons.edit')}
                          </Link>
                          <button
                            type="button"
                            className="btn btn-outline-danger"
                            onClick={toggleModal}
                          >
                            <FontAwesomeIcon icon={faTrashAlt} className="me-2" />
                            {t('details.buttons.delete')}
                          </button>
                        </>
                      )}
                    </div>
                    {profile && (
                      <>
                        {readingLoading ? (
                          <DefaultSpinner />
                        ) : (
                          <div className="currently-reading-container">
                            <h1>
                              {profile.id !== userId
                                ? t('details.currentlyReading.otherTitle', {
                                    firstName: profile.firstName,
                                  })
                                : t('details.currentlyReading.ownTitle')}
                            </h1>
                            {book !== null ? (
                              <BookListItem key={book.id} {...book} />
                            ) : (
                              t('details.currentlyReading.empty')
                            )}
                          </div>
                        )}
                        <div
                          onClick={onNavigateCurrentlyReading}
                          className="book-stats favorite-stats"
                        >
                          {t('details.shortcuts.currentlyReading')} (
                          {full ? (full.currentlyReadingBooksCount ?? 0) : 0})
                        </div>
                        <div onClick={onNavigateToRead} className="book-stats favorite-stats">
                          {t('details.shortcuts.toRead')} ({full ? (full.toReadBooksCount ?? 0) : 0}
                          )
                        </div>
                        <div onClick={onNavigateRead} className="book-stats read-stats">
                          {t('details.shortcuts.read')} ({full ? (full.readBooksCount ?? 0) : 0})
                        </div>
                      </>
                    )}
                  </>
                ) : (
                  <div className="private-profile-message">
                    <p>{t('details.privacyNotice')}</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
      <DeleteModal
        showModal={showModal}
        toggleModal={toggleModal}
        deleteHandler={async () => {
          try {
            await deleteHandler();
            showMessage(t('messages.deleteSuccess'), true);
          } catch (error) {
            const message = IsError(error) ? error.message : t('messages.deleteFailed');
            showMessage(message, false);
          }
        }}
      />
    </div>
  );
};

export default ProfileDetails;
