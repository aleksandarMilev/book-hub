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
  faPhone,
  faPlus,
  faTrashAlt,
  faUser,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';

import BookListItem from '@/features/book/components/list-item/BookListItem.js';
import { useInviteToChat } from '@/features/chat/hooks/useCrud.js';
import defaultProfilePicture from '@/features/profile/components/details/assets/default-profile-picture.png';
import { useDetails } from '@/features/profile/hooks/useCrud.js';
import type { PrivateProfile, Profile } from '@/features/profile/types/profile.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils.js';
import { useMessage } from '@/shared/stores/message/message.js';

const isFullProfile = (profile: Profile | PrivateProfile | null | undefined): profile is Profile =>
  !!profile && 'phoneNumber' in profile;

const ProfileDetails = () => {
  const {
    profile,
    userId,
    isAdmin,
    canSeePrivate,
    showModal,
    toggleModal,
    deleteHandler,
    readingList,
    readingLoading,
    chatButtons,
    chatLoading,
    chatError,
    onNavigateRead,
    onNavigateToRead,
    profileLoading,
    refetchChats,
  } = useDetails();

  const { showMessage } = useMessage();
  const inviteToChat = useInviteToChat(refetchChats);

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
                    Delete
                  </button>
                )}
                <div className="d-flex flex-column align-items-center text-center">
                  <img
                    src={profile?.imageUrl || defaultProfilePicture}
                    alt="Profile"
                    className="rounded-circle p-1 bg-primary"
                    width="110"
                  />
                  <div className="mt-3">
                    <h4>{profile ? `${profile.firstName} ${profile.lastName}` : 'Your Name'}</h4>
                  </div>
                </div>
                {canSeePrivate ? (
                  <>
                    <hr className="my-4" />
                    <ul className="list-group list-group-flush">
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faPhone} className="me-2 text-primary" />
                          Phone Number
                        </h6>
                        <span className="text-secondary">
                          {full ? full.phoneNumber || 'Your phone number' : 'Your phone number'}
                        </span>
                      </li>
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faBirthdayCake} className="me-2 text-danger" />
                          Date of Birth
                        </h6>
                        <span className="text-secondary">
                          {full ? full.dateOfBirth || 'Your date of birth' : 'Your date of birth'}
                        </span>
                      </li>
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faGlobe} className="me-2 text-info" />
                          Website
                        </h6>
                        <span className="text-secondary">
                          {full && full.socialMediaUrl ? (
                            <a href={full.socialMediaUrl} target="_blank" rel="noopener noreferrer">
                              {full.socialMediaUrl}
                            </a>
                          ) : (
                            'Your website URL'
                          )}
                        </span>
                      </li>
                      <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 className="mb-0">
                          <FontAwesomeIcon icon={faLock} className="me-2 text-warning" />
                          Account Privacy
                        </h6>
                        <span className="text-secondary">
                          {profile?.isPrivate ? 'Private' : 'Public'}
                        </span>
                      </li>
                    </ul>
                    <hr className="my-4" />
                    <div className="list-group-item d-flex flex-column align-items-start">
                      <h6 className="mb-0">
                        <FontAwesomeIcon icon={faUser} className="me-2 text-success" />
                        Biography
                      </h6>
                      <span className="text-secondary mt-2">
                        {full && full.biography ? (
                          <p className="bio-text">{full.biography}</p>
                        ) : (
                          'Your biography'
                        )}
                      </span>
                    </div>
                    <hr className="my-4" />
                    <div className="statistics-section">
                      <h6 className="mb-0">
                        <FontAwesomeIcon icon={faChartBar} className="me-2 text-info" />
                        Statistics:
                      </h6>
                      <div className="statistics-items mt-3">
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBook} className="me-2 text-info" />
                            Books Created:
                          </span>
                          <span className="stat-value">
                            {full ? (full.createdBooksCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faUser} className="me-2 text-success" />
                            Authors Created:
                          </span>
                          <span className="stat-value">
                            {full ? (full.createdAuthorsCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faComment} className="me-2 text-warning" />
                            Reviews Written:
                          </span>
                          <span className="stat-value">{full ? (full.reviewsCount ?? 0) : 0}</span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBookReader} className="me-2 text-warning" />
                            Currently Reading:
                          </span>
                          <span className="stat-value">
                            {full ? (full.currentlyReadingBooksCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBookReader} className="me-2 text-warning" />
                            Want to Read:
                          </span>
                          <span className="stat-value">
                            {full ? (full.toReadBooksCount ?? 0) : 0}
                          </span>
                        </div>
                        <div className="stat-item">
                          <span className="stat-label">
                            <FontAwesomeIcon icon={faBookReader} className="me-2 text-warning" />
                            Read:
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
                          <h3 className="chat-section-heading">Add this user to a chat:</h3>
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
                            Edit
                          </Link>
                          <button
                            type="button"
                            className="btn btn-outline-danger"
                            onClick={toggleModal}
                          >
                            <FontAwesomeIcon icon={faTrashAlt} className="me-2" />
                            Delete
                          </button>
                        </>
                      )}
                      {!profile && (
                        <Link to={routes.createProfile} className="btn btn-outline-success">
                          <FontAwesomeIcon icon={faPlus} className="me-2" />
                          Create
                        </Link>
                      )}
                    </div>
                    {profile && (
                      <>
                        {readingLoading ? (
                          <DefaultSpinner />
                        ) : (
                          <div className="currently-reading-container">
                            <h1>
                              {profile.id !== userId ? `${profile.firstName} is ` : "You're "}
                              currently reading:
                            </h1>
                            {readingList?.length > 0
                              ? readingList.map((b) => <BookListItem key={b.id} {...b} />)
                              : 'No currently reading books'}
                          </div>
                        )}
                        <div onClick={onNavigateToRead} className="book-stats favorite-stats">
                          To Read ({full ? (full.toReadBooksCount ?? 0) : 0})
                        </div>
                        <div onClick={onNavigateRead} className="book-stats read-stats">
                          Read ({full ? (full.readBooksCount ?? 0) : 0})
                        </div>
                      </>
                    )}
                  </>
                ) : (
                  <div className="private-profile-message">
                    <p>This user profile is private. Only the owner can view its full details.</p>
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
            showMessage('The profile was successfully deleted!', true);
          } catch (error) {
            const message = IsError(error) ? error.message : 'Failed to delete profile.';
            showMessage(message, false);
          }
        }}
      />
    </div>
  );
};

export default ProfileDetails;
