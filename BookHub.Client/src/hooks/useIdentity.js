import { useContext } from "react"
import { jwtDecode } from 'jwt-decode'

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
            changeAuthenticationState(result)
        } catch(error)  {
            throw error
        }
    }

    return onRegister
}