export const baseUrl = 'https://localhost:7216'
export const baseAdminUrl = baseUrl + '/administrator'

export const routes = {
    home: '/',

    register: '/identity/register',
    login: '/identity/login',
    logout: '/identity/logout',

    statistics: '/statistics',

    profile: '/profile',
    profiles: '/profiles',
    hasProfile: '/profile/exists',
    topProfiles: '/profile/topThree',
    editProfle: '/profile/edit',
    createProfle: '/profile/new',

    book: '/book',
    booksByGenre: '/book/byGenre',
    topThreeBooks: '/book/topThree',
    createBook: '/book/new',
    editBook: '/book/edit',
    genres: '/genre',

    author: '/author',
    topThreeAuthors: '/author/topThree',
    createAuthor: '/auhtor/new',
    editAuthor: '/auhtor/edit',
    authorNationalities: '/nationality',
    authorNames: '/author/names',

    searchBooks: '/search/books',
    searchArticles: '/search/articles',
    searchAuthors: '/search/authors',
    searchProfiles: '/search/profiles',
    searchChats: '/search/chats',

    review: '/review',
    vote: '/vote',

    badRequest: '/error/bad-request',
    notFound: '/error/not-found',
    accessDenied: '/access-denied',

    article: '/article',
    articles: '/articles',

    readingList: '/readingList',

    notification: '/notification',
    allNotifications: '/notification/all',
    lastThreeNotifications: '/notification/lastThree',
    markNotificationRead: '/notification/markAsRead',

    chat: '/chat',
    chats: '/chats',
    createChat: '/chat/new',
    chatsNotJoined: '/chat/chats-not-joined',
    chatMessage: '/chatMessage',

    admin: {
        createArticle: '/admin/article/new',
        editArticle: '/admin/article/edit',

        approveBook: '/book/approve',
        rejectBook: '/book/reject',

        approveAuthor: '/author/approve',
        rejectAuthor: '/author/reject',
    }
}
