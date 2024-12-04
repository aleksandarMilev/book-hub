import { createContext } from 'react'

import usePersistedState from '../hooks/usePersistedState'

export const UserContext = createContext({
    userId: '',
    username: '',
    email: '',
    token: '',
    isAdmin: false,
    isAuthenticated: false,
    hasProfile: false,
    changeAuthenticationState: (state) => {},
    changeHasProfileState: (hasProfile) => {},
    logout: () => {}
})

export function UserContextProvider(props) {
    const getInitUser = () => {
        const storedUser = localStorage.getItem('user')
        return storedUser ? JSON.parse(storedUser) : {}
    }

    const [user, setUser] = usePersistedState('user', getInitUser())

    const changeAuthenticationState = (state) => setUser(state)

    const changeHasProfileState = (hasProfile) => setUser({
        userId: user.userId,
        username: user.username,
        email: user.email,
        isAdmin: user.isAdmin,
        token: user.token,
        isAuthenticated: !!user.username,
        hasProfile: hasProfile,
        changeAuthenticationState,
        logout
    })

    const logout = () => {
        setUser({})
        localStorage.removeItem('user')
    }

    const userData = {
        userId: user.userId,
        username: user.username,
        email: user.email,
        isAdmin: user.isAdmin,
        token: user.token,
        isAuthenticated: !!user.username,
        hasProfile: user.hasProfile,
        changeAuthenticationState,
        changeHasProfileState,
        logout
    }

    return (
        <UserContext.Provider value={userData}>
            {props.children}
        </UserContext.Provider>
    )
}
