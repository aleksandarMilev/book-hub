import { useContext } from "react"
import { jwtDecode } from 'jwt-decode'

import * as profileApi from '../api/profileApi'
import * as identityApi from "../api/identityApi"
import { UserContext } from "../contexts/userContext"

export function useLogin(){
    const { changeAuthenticationState } = useContext(UserContext)

    const onLogin = async (username, password) => {
        try {
            const result = await identityApi.loginAsync(username, password)
            const tokenEncoded = jwtDecode(result.token)

            const user = {
                ...result,
                userId: tokenEncoded.nameid,
                username: tokenEncoded["unique_name"],
                isAdmin: !!tokenEncoded.role
            }

            user.hasProfile = await profileApi.hasProfileAsync(result.token)

            changeAuthenticationState(user)
        } catch (error) {
            throw error
        }
    }

    return onLogin
}

export function useRegister() {
    const { changeAuthenticationState } = useContext(UserContext)

    const onRegister = async (username, email, password) => {
        try {
            const result = await identityApi.registerAsync(username, email, password)
            const tokenEncoded = jwtDecode(result.token)

            const user = {
                ...result,
                userId: tokenEncoded.nameid,
                username: tokenEncoded["unique_name"],
                isAdmin: !!tokenEncoded.role
            }

            user.hasProfile = await profileApi.hasProfileAsync(result.token)

            changeAuthenticationState(user)
        } catch(error)  {
            throw error
        }
    }

    return onRegister
}
