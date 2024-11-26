import { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom' 
import { format } from 'date-fns'

import * as profileApi from '../api/profileApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'

export function useGet(){
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [profile, setProfile] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                const profileData = await profileApi.getAsync(token)

                if(profileData){
                    const profile = {
                        ...profileData,
                        dateOfBirth: format(new Date(profileData.dateOfBirth), 'yyyy-MM-dd')
                    }

                    setProfile(profile)
                    return
                }
               
                setProfile(null)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message} })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [token, navigate ])

    return { profile, isFetching }
}

export function useCreate(){
    const { token } = useContext(UserContext) 
    
    const navigate = useNavigate()
    
    const createHandler = async (profileData) => {
        const profile = {
            ...profileData,
            imageUrl: profileData.imageUrl || null,
            socialMediaUrl: profileData.socialMediaUrl || null,
            biography: profileData.biography || null,
        }
        
        try {
            await profileApi.createAsync(profile, token)
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
            imageUrl: profileData.imageUrl || null,
            socialMediaUrl: profileData.socialMediaUrl || null,
            biography: profileData.biography || null,
        }

        try {
            await profileApi.editAsync(profile, token)
        } catch (error) {
            navigate(routes.badRequest, { state: { message: error.message } })
        }
    }

    return editHandler
}