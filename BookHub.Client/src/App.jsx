import { Route, Routes } from "react-router-dom"
import { useState } from "react"

import { UserContext } from "./contexts/userContext"
import { routes } from "./common/constants/api"

import Header from './components/header/Header'
import Home from './components/home/Home'
import Footer from './components/footer/Footer'
import Login from './components/identity/Login'
import Register from './components/identity/Register'
import CreateBook from './components/book/CreateBook'
import BookList from "./components/book/BookList"
import BookDetails from "./components/book/BookDetails"

export default function App(){
    const[user, setUser] = useState({})

    const changeAuthenticationState = (state) => setUser(state)

    const userData = {
        userId: user.userId,
        username: user.username,
        email: user.email,
        token: user.token,
        isAuthenticated: !!user.username,
        changeAuthenticationState
    }

    return(
        <>
            <UserContext.Provider value={user}>
                <Header />
                <Routes>
                    <Route path={routes.home} element={<Home />} />
                    <Route path={routes.login} element={<Login />} />
                    <Route path={routes.register} element={<Register />} />
                    <Route path={routes.books} element={<BookList />} />
                    <Route path={routes.createBook} element={<CreateBook />} />
                    <Route path={routes.bookDetails} element={<BookDetails />} />
                </Routes>
                <Footer /> 
            </UserContext.Provider>
        </>
    )
}