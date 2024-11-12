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
import BookList from "./components/book/BookList"
import BookDetails from "./components/book/book-details/BookDetails"
import CreateBook from './components/book/CreateBook'
import EditBook from './components/book/EditBook'
import CreateAuthor from "./components/author/create-author/CreateAuthor"

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
                <Route path={routes.books + '/:id'} element={<AuthenticatedRoute element={<BookDetails />} />} />
                <Route path={routes.createBook} element={<AuthenticatedRoute element={<CreateBook />} />} />
                <Route path={routes.editBook + '/:id'} element={<AuthenticatedRoute element={<EditBook />} />} />
                <Route path={routes.createAuthor} element={<AuthenticatedRoute element={<CreateAuthor />} />} />
            </Routes>
            <Footer /> 
        </UserContextProvider>
    )
}