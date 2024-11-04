import { useContext } from "react"

import { UserContext } from "../contexts/userContext"
import * as identityApi from "../api/identityApi"

export async function useLogin(){
    const { changeAuthenticationState } = useContext(UserContext)

    async function onLogin(username, password){
        const result = await identityApi.loginAsync(username, password)
        changeAuthenticationState(result)
    }

    return onLogin;
}

export function useRegister() {
    const { changeAuthenticationState } = useContext(UserContext);

    const onRegister = async (username, email, password) => {
        try {
            const result = await identityApi.registerAsync(username, email, password);
            changeAuthenticationState(result);
            return result; 
        } catch (error) {
            throw new Error(error.message || 'An unexpected error occurred');
        }
    };

    return onRegister; 
}