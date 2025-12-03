import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import { DEFAULT_LOCALE } from '@/shared/i18n/constants.js';

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
    },
  },
});

export default i18n;
