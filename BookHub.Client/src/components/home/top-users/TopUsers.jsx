import { useContext } from 'react'
import { useNavigate } from 'react-router-dom'

import * as useProfile from '../../../hooks/useProfile'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './TopUsers.css'

export default function TopUsers() {
    const navigate = useNavigate()
    const { userId } = useContext(UserContext)
    const { profiles, isFetching, error } = useProfile.useTopThree()

    if (isFetching || !profiles) {
        return <DefaultSpinner />
    }

    if (error) {
        return <p>Error loading top users. Please try again later.</p>
    }

    const onClickHandler = (profileId) => {
        navigate(routes.profile, { state: { id: profileId === userId ? null : profileId } })
    }

    return (
        <div className="top-users-container">
            <h2>Top 3 Users</h2>
            <div className="top-users-list">
                {profiles?.map(p => (
                    <div className="top-user-card" onClick={() => onClickHandler(p.id)}>
                        <img
                            src={p.imageUrl}
                            alt={`${p.firstName} ${p.lastName}`}
                            className="user-image"
                        />
                        <div className="user-info">
                            <h4>{`${p.firstName} ${p.lastName}`}</h4>
                            <p>
                                <strong>Books Created:</strong> {p.createdBooksCount}
                            </p>
                            <p>
                                <strong>Authors Created:</strong> {p.createdAuthorsCount}
                            </p>
                            <p>
                                <strong>Reviews Written:</strong> {p.reviewsCount}
                            </p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
}
