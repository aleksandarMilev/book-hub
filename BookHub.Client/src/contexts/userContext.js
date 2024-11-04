import { createContext } from "react"

export const UserContext = createContext({
    userId: '',
    username: '',
    email: '',
    token: '',
    isAuthenticated: false,
    changeAuthenticationState: (authState = {}) => null
})