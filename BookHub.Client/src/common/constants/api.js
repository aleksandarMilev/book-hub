export const baseUrl = 'https://localhost:7216'

export const routes = {
    home: '/',
    register: '/identity/register',
    login: '/identity/login',
    logout: '/identity/logout',
    profile: '/profile',
    editProfle: '/profile/edit',
    createProfle: '/profile/new',
    books: '/books',
    topThreeBooks: '/books/topThree',
    createBook: '/books/new',
    editBook: '/books/edit',
    author: '/author',
    topThreeAuthors: '/author/topThree',
    createAuthor: '/auhtor/new',
    editAuthor: '/auhtor/edit',
    authorNationalities: '/nationality',
    authorNames: '/author/names',
    genres: '/genre',
    searchBooks: '/search/books',
    review: '/review',
    vote: '/vote',
    badRequest: '/error/bad-request',
    notFound: '/error/not-found',
    accessDenied: '/access-denied',
    admin: {
        createArticle: '/admin/article/new'
    }
}
