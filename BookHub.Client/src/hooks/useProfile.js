import { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom' 

import * as profileApi from '../api/profileApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useGet(){
    const { token, userId } = useContext(UserContext)

    const navigate = useNavigate()
    const [profile, setProfile] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                setProfile(await profileApi.getAsync(token, userId))
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message} })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [token, userId, navigate ])

    return { profile, isFetching }
}

export function useCreate(){
    const { token } = useContext(UserContext) 
    
    const navigate = useNavigate()

    const createHandler = async (profileData) => {
        const profile = {
            ...profileData,
            imageUrl: bookData.profileData || null,
            socialMediaUrl: bookData.socialMediaUrl || null
        }

        try {
            const profileId = await profileApi.createAsync(profile, token)
            return profileId
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return createHandler
}

export function useEdit(){
    const { token } = useContext(UserContext) 

    const navigate = useNavigate()

    const editHandler = async (profileData) => {
        const profile = {
            ...profileData,
            imageUrl: bookData.profileData || null,
            socialMediaUrl: bookData.socialMediaUrl || null
        }

        try {
            const isSuccessful = await profileApi.editAsync(profile, token)
            return isSuccessful
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return editHandler
}