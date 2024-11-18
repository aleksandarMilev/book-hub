import { useContext } from "react"

import * as identityApi from "../api/identityApi"
import { UserContext } from "../contexts/userContext"
import { errors } from "../common/constants/messages"

export function useLogin(){
    const { changeAuthenticationState } = useContext(UserContext)

    const onLogin = async (username, password) => {
        try {
            const result = await identityApi.loginAsync(username, password)
            changeAuthenticationState(result)
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