import { createContext } from "react"

import usePersistedState from '../hooks/usePersistedState'

export const UserContext = createContext({
    userId: '',
    username: '',
    email: '',
    token: '',
    isAuthenticated: false,
    changeAuthenticationState: (state) => {},
    logout: () => {}
})

export function UserContextProvider(props) {
    function getInitUser(){
        const storedUser = localStorage.getItem('user')
        return storedUser ? JSON.parse(storedUser) : {}
    }

    const [user, setUser] = usePersistedState('user', getInitUser())

    const changeAuthenticationState = (state) => setUser(state)
    
    const logout = () => setUser({})

    const userData = {
        userId: user.userId,
        username: user.username,
        email: user.email,
        token: user.token,
        isAuthenticated: !!user.username,
        changeAuthenticationState,
        logout
    }

    return (
        <UserContext.Provider value={userData}>
            {props.children}
        </UserContext.Provider>
    )
}
