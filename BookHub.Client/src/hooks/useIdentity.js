import { useContext } from "react"

import { UserContext } from "../contexts/userContext"
import * as identityApi from "../api/identityApi"

export function useLogin(){
    const { changeAuthenticationState } = useContext(UserContext)

    const onLogin = async (username, password) => {
        try {
            const result = await identityApi.loginAsync(username, password)
            changeAuthenticationState(result)
        } catch (error) {
            throw new Error(error.message || 'An unexpected error occurred')
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
        } catch (error) {
            throw new Error(error.message || 'An unexpected error occurred')
        }
    }

    return onRegister
}