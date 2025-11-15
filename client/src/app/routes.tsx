/* eslint-disable react-refresh/only-export-components */

import React, { lazy, Suspense } from 'react';
import { createBrowserRouter } from 'react-router-dom';

import AdminRoute from '@/app/routes/guards/AdminRoute';
import AuthenticatedRoute from '@/app/routes/guards/AuthenticatedRoute';
import ChatRoute from '@/app/routes/guards/ChatRoute';
import ProfileRoute from '@/app/routes/guards/ProfileRoute';
import { routes } from '@/shared/lib/constants/api';

import App from './App';
import Loading from './Loading';

const Fallback = () => <Loading />;

const Home = lazy(() => import('@/features/home/components/Home'));

const Login = lazy(() => import('@/features/identity/components/login/Login'));
const Register = lazy(() => import('@/features/identity/components/register/Register'));
const Logout = lazy(() => import('@/features/identity/components/logout/Logout'));

const ProfileList = lazy(() => import('@/features/profile/components/list/ProfileList'));
const ProfileDetails = lazy(() => import('@/features/profile/components/details/ProfileDetails'));
const CreateProfile = lazy(() => import('@/features/profile/components/create/CreateProfile'));
const EditProfile = lazy(() => import('@/features/profile/components/edit/EditProfile'));

const BookList = lazy(() => import('@/features/book/components/list/BookList'));
const BookDetails = lazy(() => import('@/features/book/components/details/BookDetails'));
const CreateBook = lazy(() => import('@/features/book/components/create/CreateBook'));
const EditBook = lazy(() => import('@/features/book/components/edit/EditBook'));

const ReadingList = lazy(() => import('@/features/reading-list/components/list/ReadingList'));
const ReviewList = lazy(() => import('@/features/review/components/review-list/ReviewList'));

const GenreDetails = lazy(() => import('@/features/genre/components/details/GenreDetails'));

const AuthorList = lazy(() => import('@/features/author/components/author-list/AuthorList'));
const AuthorDetails = lazy(
  () => import('@/features/author/components/author-details/AuthorDetails'),
);
const CreateAuthor = lazy(() => import('@/features/author/components/create-author/CreateAuthor'));
const EditAuthor = lazy(() => import('@/features/author/components/edit-author/EditAuthor'));

const ArticleList = lazy(() => import('@/features/article/components/list/ArticleList'));
const ArticleDetails = lazy(() => import('@/features/article/components/details/ArticleDetails'));
const CreateArticle = lazy(() => import('@/features/article/components/create/CreateArticle'));
const EditArticle = lazy(() => import('@/features/article/components/edit/EditArticle'));

const NotificationList = lazy(
  () => import('@/features/notification/components/list/NotificationList'),
);

const ChatList = lazy(() => import('@/components/chat/chat-list/ChatList'));
const ChatDetails = lazy(() => import('@/components/chat/chat-details/ChatDetails'));

const BadRequest = lazy(() => import('@/shared/components/errors/bad-request/BadRequest'));
const NotFound = lazy(() => import('@/shared/components/errors/not-found/NotFound'));
const AccessDenied = lazy(() => import('@/shared/components/errors/access-denied/AccessDenied'));

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
        path: routes.createProfile,
        element: withSuspense(<AuthenticatedRoute element={<CreateProfile />} />),
      },
      {
        path: routes.editProfile,
        element: withSuspense(<ProfileRoute element={<EditProfile />} />),
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
        path: routes.createBook,
        element: withSuspense(<ProfileRoute element={<CreateBook />} />),
      },
      {
        path: `${routes.editBook}/:id`,
        element: withSuspense(<ProfileRoute element={<EditBook />} />),
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
        path: `${routes.genres}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<GenreDetails />} />),
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
        path: routes.createAuthor,
        element: withSuspense(<ProfileRoute element={<CreateAuthor />} />),
      },
      {
        path: `${routes.editAuthor}/:id`,
        element: withSuspense(<ProfileRoute element={<EditAuthor />} />),
      },

      {
        path: routes.articles,
        element: withSuspense(<AuthenticatedRoute element={<ArticleList />} />),
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
        path: `${routes.article}/:id`,
        element: withSuspense(<AuthenticatedRoute element={<ArticleDetails />} />),
      },

      {
        path: routes.notification,
        element: withSuspense(<AuthenticatedRoute element={<NotificationList />} />),
      },

      {
        path: routes.chats,
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
