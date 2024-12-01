import { useState, useContext } from 'react'
import { Link, useNavigate, useLocation } from 'react-router-dom'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import {
    faPhone,
    faBirthdayCake,
    faUser,
    faGlobe,
    faLock,
    faEdit,
    faTrashAlt,
    faPlus,
    faBook,
    faComment,
    faChartBar
} from '@fortawesome/free-solid-svg-icons'

import * as useProfile from '../../../hooks/useProfile'
import * as profileApi from '../../../api/profileApi'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'
import DeleteModal from  '../../common/delete-modal/DeleteModal'

import defaultProfilePicture from '../../../assets/images/defaultProfilePicture.png'

import './ProfileDetails.css'

export default function ProfileDetails() {
    const location = useLocation()

    const { token, userId } = useContext(UserContext)
    const navigate = useNavigate()

    const { profile, isFetching } = location?.state?.id 
        ? useProfile.useOtherProfile(location?.state?.id) 
        : useProfile.useMineProfile()

    const [showModal, setShowModal] = useState(false)
    const toggleModal = () => setShowModal(old => !old)

    async function deleteHandler() {
        if(showModal){
            try {
                await profileApi.deleteAsync(token)
                navigate(routes.home)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message } })
            } finally {
                toggleModal()
            }
        } else {
            toggleModal()
        }
    }

    if (isFetching) {
        return <DefaultSpinner />
    }

    console.log(profile);
    

    return (
        <div className="profile-details container-fluid">
            <div className="main-body">
                <div className="row align-items-center justify-content-center">
                    <div className="col-lg-6">
                        <div className="card">
                            <div className="card-body">
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
                                <hr className="my-4" />
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                        <h6 className="mb-0">
                                            <FontAwesomeIcon icon={faPhone} className="me-2 text-primary" />
                                            Phone Number
                                        </h6>
                                        <span className="text-secondary">{profile?.phoneNumber || 'Your phone number'}</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                        <h6 className="mb-0">
                                            <FontAwesomeIcon icon={faBirthdayCake} className="me-2 text-danger" />
                                            Date of Birth
                                        </h6>
                                        <span className="text-secondary">{profile?.dateOfBirth || 'Your date of birth'}</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                        <h6 className="mb-0">
                                            <FontAwesomeIcon icon={faGlobe} className="me-2 text-info" />
                                            Website
                                        </h6>
                                        <span className="text-secondary">
                                            {profile?.socialMediaUrl ? (
                                                <a href={profile.socialMediaUrl} target="_blank" rel="noopener noreferrer">
                                                    {profile.socialMediaUrl}
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
                                        <span className="text-secondary">{profile?.isPrivate ? 'Private' : 'Public'}</span>
                                    </li>
                                </ul>
                                <hr className="my-4" />
                                <div className="list-group-item d-flex flex-column align-items-start">
                                    <h6 className="mb-0">
                                        <FontAwesomeIcon icon={faUser} className="me-2 text-success" />
                                        Biography
                                    </h6>
                                    <span className="text-secondary mt-2">
                                        {profile?.biography ? (
                                            <p className="bio-text">{profile.biography}</p>
                                        ) : (
                                            'Your biography'
                                        )}
                                    </span>
                                </div>
                                <hr className="my-4" />
                                <div className="statistics-section">
                                    <h6 className="mb-0">
                                        <FontAwesomeIcon icon={faChartBar} className="me-2 text-info" />
                                        Statistics
                                    </h6>
                                    <div className="statistics-items mt-3">
                                        <div className="stat-item">
                                                <span className="stat-label">
                                                    <FontAwesomeIcon icon={faBook} className="me-2 text-info" />
                                                    Books Created:
                                                </span>
                                                <span className="stat-value">{profile?.createdBooksCount || 0}</span>
                                            </div>
                                            <div className="stat-item">
                                                <span className="stat-label">
                                                    <FontAwesomeIcon icon={faUser} className="me-2 text-success" />
                                                    Authors Created:
                                                </span>
                                                <span className="stat-value">{profile?.createdAuthorsCount || 0}</span>
                                            </div>
                                            <div className="stat-item">
                                                <span className="stat-label">
                                                    <FontAwesomeIcon icon={faComment} className="me-2 text-warning" />
                                                    Reviews Written:
                                                </span>
                                                <span className="stat-value">{profile?.reviewsCount || 0}</span>
                                            </div>
                                        </div>
                                    </div>
                                <hr className="my-4" />
                                <div className="d-flex justify-content-around mt-3">
                                    {profile && userId === profile.id ? (
                                        <>
                                            <Link to={routes.editProfle} className="btn btn-outline-primary">
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
                                    ) : (
                                        <Link to={routes.createProfle} className="btn btn-outline-success">
                                            <FontAwesomeIcon icon={faPlus} className="me-2" />
                                            Create
                                        </Link>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
                <DeleteModal
                    showModal={showModal}
                    toggleModal={toggleModal}
                    deleteHandler={deleteHandler}
                />
        </div>
    )
}
