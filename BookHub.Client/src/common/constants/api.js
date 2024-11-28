export const baseUrl = 'https://localhost:7216'
export const baseAdminUrl = baseUrl + '/administrator'

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
    genres: '/genre',

    author: '/author',
    topThreeAuthors: '/author/topThree',
    createAuthor: '/auhtor/new',
    editAuthor: '/auhtor/edit',
    authorNationalities: '/nationality',
    authorNames: '/author/names',

    searchBooks: '/search/books',
    searchArticles: '/search/articles',

    review: '/review',
    vote: '/vote',

    badRequest: '/error/bad-request',
    notFound: '/error/not-found',
    accessDenied: '/access-denied',

    article: '/article',
    articles: '/articles',

    admin: {
        createArticle: '/admin/article/new',
        editArticle: '/admin/article/edit',

        approveBook: '/book/approve',
        rejectBook: '/book/reject',

        approveAuthor: '/author/approve',
        rejectAuthor: '/author/reject',
    }
}
