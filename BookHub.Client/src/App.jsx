import { Route, Routes } from "react-router-dom"

import AuthenticatedRoute from './components/common/AuthenticatedRoute'
import { routes } from "./common/constants/api"
import { UserContextProvider } from "./contexts/userContext"

import Header from './components/header/Header'
import Home from './components/home/Home'
import Footer from './components/footer/Footer'
import Login from './components/identity/Login'
import Register from './components/identity/Register'
import Logout from './components/identity/Logout'
import CreateBook from './components/book/CreateBook'
import BookList from "./components/book/BookList"
import BookDetails from "./components/book/BookDetails"

export default function App(){
    return(
        <UserContextProvider>
            <Header />
            <Routes>
                <Route path={routes.home} element={<Home />} />
                <Route path={routes.login} element={<Login />} />
                <Route path={routes.register} element={<Register />} />
                <Route path={routes.logout} element={<Logout />} />
                <Route path={routes.books} element={<AuthenticatedRoute element={<BookList />} />} />
                <Route path={routes.createBook} element={<AuthenticatedRoute element={<CreateBook />} />} />
                <Route path={routes.bookDetails} element={<AuthenticatedRoute element={<BookDetails />} />} />
            </Routes>
            <Footer /> 
        </UserContextProvider>
    )
}