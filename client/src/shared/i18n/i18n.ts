import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import { DEFAULT_LOCALE } from '@/shared/i18n/constants';

const bgLayout = await import('@/shared/i18n/locales/bg/layout.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgCommon = await import('@/shared/i18n/locales/bg/common.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgHome = await import('@/shared/i18n/locales/bg/home.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgArticles = await import('@/shared/i18n/locales/bg/articles.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgAuthors = await import('@/shared/i18n/locales/bg/authors.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgIdentity = await import('@/shared/i18n/locales/bg/identity.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgBooks = await import('@/shared/i18n/locales/bg/books.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgGenres = await import('@/shared/i18n/locales/bg/genres.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgProfiles = await import('@/shared/i18n/locales/bg/profiles.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgReadingList = await import('@/shared/i18n/locales/bg/readingList.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgNotifications = await import('@/shared/i18n/locales/bg/notifications.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgReviews = await import('@/shared/i18n/locales/bg/reviews.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgChats = await import('@/shared/i18n/locales/bg/chats.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgLegal = await import('@/shared/i18n/locales/bg/legal.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const bgChallenges = await import('@/shared/i18n/locales/bg/challenges.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enLayout = await import('@/shared/i18n/locales/en/layout.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enCommon = await import('@/shared/i18n/locales/en/common.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enHome = await import('@/shared/i18n/locales/en/home.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enArticles = await import('@/shared/i18n/locales/en/articles.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enAuthors = await import('@/shared/i18n/locales/en/authors.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enIdentity = await import('@/shared/i18n/locales/en/identity.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enBooks = await import('@/shared/i18n/locales/en/books.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enGenres = await import('@/shared/i18n/locales/en/genres.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enProfiles = await import('@/shared/i18n/locales/en/profiles.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enReadingList = await import('@/shared/i18n/locales/en/readingList.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enNotifications = await import('@/shared/i18n/locales/en/notifications.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enReviews = await import('@/shared/i18n/locales/en/reviews.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enChats = await import('@/shared/i18n/locales/en/chats.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enLegal = await import('@/shared/i18n/locales/en/legal.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const enChallenges = await import('@/shared/i18n/locales/en/challenges.json', {
  assert: { type: 'json' },
}).then((mod) => mod.default);

const storedLanguage = typeof window !== 'undefined' ? localStorage.getItem('language') : null;

const initialLanguage = storedLanguage || DEFAULT_LOCALE;

void i18n.use(initReactI18next).init({
  lng: initialLanguage,
  fallbackLng: 'en-US',
  supportedLngs: ['bg-BG', 'en-US'],
  interpolation: {
    escapeValue: false,
  },
  resources: {
    'en-US': {
      layout: enLayout,
      common: enCommon,
      home: enHome,
      articles: enArticles,
      identity: enIdentity,
      authors: enAuthors,
      books: enBooks,
      genres: enGenres,
      profiles: enProfiles,
      readingList: enReadingList,
      notifications: enNotifications,
      reviews: enReviews,
      chats: enChats,
      legal: enLegal,
      challenges: enChallenges,
    },
    'bg-BG': {
      layout: bgLayout,
      common: bgCommon,
      home: bgHome,
      articles: bgArticles,
      identity: bgIdentity,
      authors: bgAuthors,
      books: bgBooks,
      genres: bgGenres,
      profiles: bgProfiles,
      readingList: bgReadingList,
      notifications: bgNotifications,
      reviews: bgReviews,
      chats: bgChats,
      legal: bgLegal,
      challenges: bgChallenges,
    },
  },
});

export default i18n;
