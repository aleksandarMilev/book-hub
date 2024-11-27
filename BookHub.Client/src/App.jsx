import { Route, Routes } from "react-router-dom"

import AuthenticatedRoute from './components/common/authenticated-route/AuthenticatedRoute'
import AdminRoute from './components/common/admin-route/AdminRoute'
import { routes } from "./common/constants/api"
import { UserContextProvider } from "./contexts/userContext"

import Header from './components/common/header/Header'
import Footer from './components/common/footer/Footer'
import Home from './components/home/Home'

import Login from './components/identity/login/Login'
import Register from './components/identity/register/Register'
import Logout from './components/identity/logout/Logout'

import ProfileDetails from "./components/profile/profile-details/ProfileDetails"
import CreateProfile from "./components/profile/create-profile/CreateProfile"
import EditProfile from "./components/profile/edit-profile/EditProfile"

import BookList from "./components/book/book-list/BookList"
import BookDetails from "./components/book/book-details/BookDetails"
import CreateBook from './components/book/create-book/CreateBook'
import EditBook from './components/book/edit-book/EditBook'

import AuthorDetails from "./components/author/author-details/AuthorDetails"
import CreateAuthor from "./components/author/create-author/CreateAuthor"
import EditAuthor from "./components/author/edit-author/EditAuthor"

import ReviewList from "./components/review/review-list/ReviewList"

import BadRequest from "./components/common/error/BadRequest"
import NotFound from "./components/common/error/NotFound"
import AccessDenied from "./components/common/error/AccessDenied"

import ArticleForm from "./components/article/article-form/ArticleForm"

export default function App(){
    return(
        <UserContextProvider>
            <Header />
            <Routes>
                <Route path={routes.home} element={<Home />} />

                <Route path={routes.login} element={<Login />} />
                <Route path={routes.register} element={<Register />} />
                <Route path={routes.logout} element={<Logout />} />

                <Route path={routes.profile} element={<AuthenticatedRoute element={<ProfileDetails />} />} />
                <Route path={routes.createProfle} element={<AuthenticatedRoute element={<CreateProfile />} />} />
                <Route path={routes.editProfle} element={<AuthenticatedRoute element={<EditProfile />} />} />

                <Route path={routes.books} element={<AuthenticatedRoute element={<BookList />} />} />
                <Route path={routes.books + '/:id'} element={<AuthenticatedRoute element={<BookDetails />} />} />
                <Route path={routes.createBook} element={<AuthenticatedRoute element={<CreateBook />} />} />
                <Route path={routes.editBook + '/:id'} element={<AuthenticatedRoute element={<EditBook />} />} />

                <Route path={routes.review + '/:bookId'} element={<AuthenticatedRoute element={<ReviewList />} />} />

                <Route path={routes.author + '/:id'} element={<AuthenticatedRoute element={<AuthorDetails />} />} />
                <Route path={routes.createAuthor} element={<AuthenticatedRoute element={<CreateAuthor />} />} />
                <Route path={routes.editAuthor + '/:id'} element={<AuthenticatedRoute element={<EditAuthor />} />} />

                <Route path={routes.admin.createArticle} element={<AdminRoute element={<ArticleForm />} />} />

                <Route path={routes.badRequest} element={<BadRequest />} />
                <Route path={routes.notFound} element={<NotFound />} />
                <Route path={routes.accessDenied} element={<AccessDenied />} />
            </Routes>
            <Footer /> 
        </UserContextProvider>
    )
}