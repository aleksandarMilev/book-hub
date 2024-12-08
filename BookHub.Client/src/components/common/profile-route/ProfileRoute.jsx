import { useContext } from 'react'
import { Navigate } from 'react-router-dom'

import { UserContext } from '../../../contexts/userContext'
import { routes } from '../../../common/constants/api'

export default function ProfileRoute({ element }){
    const { isAuthenticated, hasProfile, isAdmin } = useContext(UserContext)

    if(!isAuthenticated){
        return <Navigate to={routes.login} replace />
    }

    if(!hasProfile && !isAdmin){
        return <Navigate to={routes.profile} replace />
    }

    return element
}
