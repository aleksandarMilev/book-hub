/* eslint-disable react-refresh/only-export-components */
import React, { lazy, Suspense } from 'react';
import { createBrowserRouter } from 'react-router-dom';

import App from '@/app/App.js';
import Loading from '@/app/layout/loading/Loading.js';
import AdminRoute from '@/app/routes/guards/admin/AdminRoute.js';
import AuthenticatedRoute from '@/app/routes/guards/authenticated/AuthenticatedRoute.js';
import ChatRoute from '@/app/routes/guards/chat/ChatRoute.js';
import { routes } from '@/shared/lib/constants/api.js';

const Fallback = () => <Loading />;

const Home = lazy(() => import('@/features/home/components/Home.jsx'));

const Login = lazy(() => import('@/features/identity/components/login/Login.jsx'));
const Register = lazy(() => import('@/features/identity/components/register/Register.jsx'));
const Logout = lazy(() => import('@/features/identity/components/logout/Logout.jsx'));

const ProfileList = lazy(() => import('@/features/profile/components/list/ProfileList.jsx'));
const ProfileDetails = lazy(
  () => import('@/features/profile/components/details/ProfileDetails.jsx'),
);
const EditProfile = lazy(() => import('@/features/profile/components/edit/EditProfile.jsx'));

const BookList = lazy(() => import('@/features/book/components/list/BookList.jsx'));
const BookDetails = lazy(() => import('@/features/book/components/details/BookDetails.jsx'));
const CreateBook = lazy(() => import('@/features/book/components/create/CreateBook.jsx'));
const EditBook = lazy(() => import('@/features/book/components/edit/EditBook.jsx'));

const ReadingList = lazy(() => import('@/features/reading-list/components/list/ReadingList.jsx'));
const ReviewList = lazy(() => import('@/features/review/components/list/ReviewList.js'));

const GenreList = lazy(() => import('@/features/genre/components/list/GenreList.js'));
const GenreDetails = lazy(() => import('@/features/genre/components/details/GenreDetails.jsx'));

const AuthorList = lazy(() => import('@/features/author/components/list/AuthorList.js'));
const AuthorDetails = lazy(() => import('@/features/author/components/details/AuthorDetails.js'));
const CreateAuthor = lazy(() => import('@/features/author/components/create/CreateAuthor.js'));
const EditAuthor = lazy(() => import('@/features/author/components/edit/EditAuthor.js'));

const ArticleList = lazy(() => import('@/features/article/components/list/ArticleList.jsx'));
const ArticleDetails = lazy(
  () => import('@/features/article/components/details/ArticleDetails.jsx'),
);
const CreateArticle = lazy(() => import('@/features/article/components/create/CreateArticle.jsx'));
const EditArticle = lazy(() => import('@/features/article/components/edit/EditArticle.jsx'));

const NotificationList = lazy(
  () => import('@/features/notification/components/list/NotificationList.jsx'),
);

const CreateChat = lazy(() => import('@/features/chat/components/create/CreateChat.jsx'));
const EditChat = lazy(() => import('@/features/chat/components/edit/EditChat.jsx'));
const ChatList = lazy(() => import('@/features/chat/components/list/ChatList.jsx'));
const ChatDetails = lazy(() => import('@/features/chat/components/details/ChatDetails.jsx'));

const BadRequest = lazy(() => import('@/shared/components/errors/bad-request/BadRequest.jsx'));
const NotFound = lazy(() => import('@/shared/components/errors/not-found/NotFound.jsx'));
const AccessDenied = lazy(
  () => import('@/shared/components/errors/access-denied/AccessDenied.jsx'),
);

const withSuspense = (element: React.JSX.Element) => (
  <Suspense fallback={<Fallback />}>{element}</Suspense>
);

export const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      { index: true, element: withSuspense(<Home />) },

      { path: routes.login, element: withSuspense(<Login />) },
      { path: routes.register, element: withSuspense(<Register />) },
      { path: routes.logout, element: withSuspense(<Logout />) },

      {
        path: routes.profiles,
        element: withSuspense(<AuthenticatedRoute element={<ProfileList />} />),
      },
      {
        path: routes.profile,
        element: withSuspense(<AuthenticatedRoute element={<ProfileDetails />} />),
      },
      {
        path: routes.editProfile,
        element: withSuspense(<AuthenticatedRoute element={<EditProfile />} />),
      },

      {
        path: routes.book,
        element: withSuspense(<AuthenticatedRoute element={<BookList />} />),
      },
      {
        path: `${routes.book}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<BookDetails />} />),
      },
      {
        path: `${routes.book}/:id/:slug`,
        element: withSuspense(<AuthenticatedRoute element={<BookDetails />} />),
      },
      {
        path: routes.createBook,
        element: withSuspense(<AuthenticatedRoute element={<CreateBook />} />),
      },
      {
        path: `${routes.editBook}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<EditBook />} />),
      },

      {
        path: routes.readingList,
        element: withSuspense(<AuthenticatedRoute element={<ReadingList />} />),
      },
      {
        path: `${routes.review}/:bookId`,
        element: withSuspense(<AuthenticatedRoute element={<ReviewList />} />),
      },

      {
        path: `${routes.genres}/:id/:slug`,
        element: withSuspense(<AuthenticatedRoute element={<GenreDetails />} />),
      },
      {
        path: routes.genres,
        element: withSuspense(<AuthenticatedRoute element={<GenreList />} />),
      },

      {
        path: routes.author,
        element: withSuspense(<AuthenticatedRoute element={<AuthorList />} />),
      },
      {
        path: `${routes.author}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<AuthorDetails />} />),
      },
      {
        path: `${routes.author}/:id/:slug`,
        element: withSuspense(<AuthenticatedRoute element={<AuthorDetails />} />),
      },
      {
        path: routes.createAuthor,
        element: withSuspense(<AuthenticatedRoute element={<CreateAuthor />} />),
      },
      {
        path: `${routes.editAuthor}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<EditAuthor />} />),
      },

      {
        path: routes.articles,
        element: withSuspense(<ArticleList />),
      },
      {
        path: routes.admin.createArticle,
        element: withSuspense(<AdminRoute element={<CreateArticle />} />),
      },
      {
        path: `${routes.admin.editArticle}/:id`,
        element: withSuspense(<AdminRoute element={<EditArticle />} />),
      },
      {
        path: `${routes.articles}/:id/:slug`,
        element: withSuspense(<ArticleDetails />),
      },

      {
        path: routes.notification,
        element: withSuspense(<AuthenticatedRoute element={<NotificationList />} />),
      },

      {
        path: routes.createChat,
        element: withSuspense(<AuthenticatedRoute element={<CreateChat />} />),
      },
      {
        path: `${routes.editChat}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<EditChat />} />),
      },
      {
        path: routes.chat,
        element: withSuspense(<AuthenticatedRoute element={<ChatList />} />),
      },
      {
        path: `${routes.chat}/:id`,
        element: withSuspense(<ChatRoute element={<ChatDetails />} />),
      },

      { path: routes.badRequest, element: withSuspense(<BadRequest />) },
      { path: routes.notFound, element: withSuspense(<NotFound />) },
      { path: routes.accessDenied, element: withSuspense(<AccessDenied />) },
      { path: '*', element: withSuspense(<NotFound />) },
    ],
  },
]);
