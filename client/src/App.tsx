import { Route, Routes } from 'react-router-dom';

import { routes } from './common/constants/api';
import ArticleDetails from './components/article/article-details/ArticleDetails';
import ArticleList from './components/article/article-list/ArticleList';
import CreateArticle from './components/article/create-article/CreateArticle';
import EditArticle from './components/article/edit-article/EditArticle';
import AuthorDetails from './components/author/author-details/AuthorDetails';
import AuthorList from './components/author/author-list/AuthorList';
import CreateAuthor from './components/author/create-author/CreateAuthor';
import EditAuthor from './components/author/edit-author/EditAuthor';
import BookDetails from './components/book/book-details/BookDetails';
import BookList from './components/book/book-list/BookList';
import CreateBook from './components/book/create-book/CreateBook';
import EditBook from './components/book/edit-book/EditBook';
import ChatDetails from './components/chat/chat-details/ChatDetails';
import ChatList from './components/chat/chat-list/ChatList';
import CreateChat from './components/chat/create-chat/CreateChat';
import EditChat from './components/chat/edit-chat/EditChat';
import AccessDenied from './components/common/error/AccessDenied';
import BadRequest from './components/common/error/BadRequest';
import NotFound from './components/common/error/NotFound';
import Footer from './components/common/footer/Footer';
import Header from './components/common/header/Header';
import AdminRoute from './components/common/routes/admin-route/AdminRoute';
import AuthenticatedRoute from './components/common/routes/authenticated-route/AuthenticatedRoute';
import ChatRoute from './components/common/routes/chat-route/ChatRoute';
import ProfileRoute from './components/common/routes/profile-route/ProfileRoute';
import GenreDetails from './components/genre/genre-details/GenreDetails';
import Home from './components/home/Home';
import Login from './components/identity/login/Login';
import Logout from './components/identity/logout/Logout';
import Register from './components/identity/register/Register';
import NotificationList from './components/notifications/notification-list/NotificationList';
import CreateProfile from './components/profile/create-profile/CreateProfile';
import EditProfile from './components/profile/edit-profile/EditProfile';
import ProfileDetails from './components/profile/profile-details/ProfileDetails';
import ProfileList from './components/profile/profile-list/ProfileList';
import ReadingList from './components/reading-list/ReadingList';
import ReviewList from './components/review/review-list/ReviewList';
import { MessageProvider } from './contexts/message/messageContext';
import { UserContextProvider } from './contexts/user/userContext';

export default function App() {
  return (
    <UserContextProvider>
      <MessageProvider>
        <Header />
        <Routes>
          <Route path={routes.home} element={<Home />} />
          <Route path={routes.login} element={<Login />} />
          <Route path={routes.register} element={<Register />} />
          <Route path={routes.logout} element={<Logout />} />
          <Route
            path={routes.profiles}
            element={<AuthenticatedRoute element={<ProfileList />} />}
          />
          <Route
            path={routes.profile}
            element={<AuthenticatedRoute element={<ProfileDetails />} />}
          />
          <Route
            path={routes.createProfile}
            element={<AuthenticatedRoute element={<CreateProfile />} />}
          />
          <Route path={routes.editProfile} element={<ProfileRoute element={<EditProfile />} />} />
          <Route path={routes.book} element={<AuthenticatedRoute element={<BookList />} />} />
          <Route
            path={routes.book + '/:id'}
            element={<AuthenticatedRoute element={<BookDetails />} />}
          />
          <Route path={routes.createBook} element={<ProfileRoute element={<CreateBook />} />} />
          <Route
            path={routes.editBook + '/:id'}
            element={<ProfileRoute element={<EditBook />} />}
          />
          <Route
            path={routes.readingList}
            element={<AuthenticatedRoute element={<ReadingList />} />}
          />
          <Route
            path={routes.review + '/:bookId'}
            element={<AuthenticatedRoute element={<ReviewList />} />}
          />
          <Route
            path={routes.genres + '/:id'}
            element={<AuthenticatedRoute element={<GenreDetails />} />}
          />
          <Route path={routes.author} element={<AuthenticatedRoute element={<AuthorList />} />} />
          <Route
            path={routes.author + '/:id'}
            element={<AuthenticatedRoute element={<AuthorDetails />} />}
          />
          <Route path={routes.createAuthor} element={<ProfileRoute element={<CreateAuthor />} />} />
          <Route
            path={routes.editAuthor + '/:id'}
            element={<ProfileRoute element={<EditAuthor />} />}
          />
          <Route
            path={routes.articles}
            element={<AuthenticatedRoute element={<ArticleList />} />}
          />
          <Route
            path={routes.admin.createArticle}
            element={<AdminRoute element={<CreateArticle />} />}
          />
          <Route
            path={routes.admin.editArticle + '/:id'}
            element={<AdminRoute element={<EditArticle />} />}
          />
          <Route
            path={routes.article + '/:id'}
            element={<AuthenticatedRoute element={<ArticleDetails />} />}
          />
          <Route
            path={routes.notification}
            element={<AuthenticatedRoute element={<NotificationList />} />}
          />
          <Route path={routes.chats} element={<AuthenticatedRoute element={<ChatList />} />} />
          <Route path={routes.chat + '/:id'} element={<ChatRoute element={<ChatDetails />} />} />
          <Route path={routes.createChat} element={<ProfileRoute element={<CreateChat />} />} />
          <Route path={routes.editChat} element={<AuthenticatedRoute element={<EditChat />} />} />
          <Route path={routes.badRequest} element={<BadRequest />} />
          <Route path={routes.notFound} element={<NotFound />} />
          <Route path={routes.accessDenied} element={<AccessDenied />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
        <Footer />
      </MessageProvider>
    </UserContextProvider>
  );
}
