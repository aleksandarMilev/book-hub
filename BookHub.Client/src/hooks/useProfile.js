import { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom' 
import { format } from 'date-fns'

import * as profileApi from '../api/profileApi'
import { routes } from '../common/constants/api'
import { UserContext } from '../contexts/userContext'


export function useTopThree(){
    const [profiles, setProfiles] = useState(null)
    const [isFetching, setIsFetching] = useState(false)
    const [error, setError] = useState(null)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                setProfiles(await profileApi.topThreeAsync())
            } catch (error) {
                setError(error)
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [])

    return { profiles, isFetching, error }
}

export function useMineProfile(){
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [profile, setProfile] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)
                const profileData = await profileApi.mineAsync(token)

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

export function useOtherProfile(id){
    const { token } = useContext(UserContext)

    const navigate = useNavigate()
    const [profile, setProfile] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        async function fetchData() {
            try {
                setIsFetching(true)

                const profileData = await profileApi.otherAsync(id, token)
                const profile = {
                    ...profileData,
                    dateOfBirth: profileData.dateOfBirth 
                        ? format(new Date(profileData.dateOfBirth), 'yyyy-MM-dd') 
                        : null
                }

                setProfile(profile)
            } catch (error) {
                navigate(routes.badRequest, { state: { message: error.message} })
            } finally {
                setIsFetching(false)
            }
        }

        fetchData()
    }, [token, navigate, id])

    return { profile, isFetching }
}

export function useCreate(){
    const { token, changeHasProfileState } = useContext(UserContext) 
    
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
            changeHasProfileState(true)
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