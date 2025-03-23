export const baseUrl = import.meta?.env?.VITE_REACT_APP_SERVER_URL ?? "http://localhost:8080"
export const baseAdminUrl = baseUrl + '/administrator'

export const routes = {
    home: '/',

    register: '/identity/register',
    login: '/identity/login',
    logout: '/identity/logout',

    statistics: '/statistics',

    profile: '/profile',
    mineProfile: '/profile/mine',
    profiles: '/profiles',
    hasProfile: '/profile/exists',
    topProfiles: '/profile/top',
    editProfle: '/profile/edit',
    createProfle: '/profile/new',

    book: '/book',
    booksByGenre: '/book/genre',
    booksByAuthor: '/book/author',
    topThreeBooks: '/book/top',
    createBook: '/book/new',
    editBook: '/book/edit',
    genres: '/genre',

    author: '/author',
    topThreeAuthors: '/author/top',
    createAuthor: '/auhtor/new',
    editAuthor: '/auhtor/edit',
    authorNationalities: '/nationality/names',
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
    lastThreeNotifications: '/notification/last',
    markNotificationRead: '/notification/markAsRead',

    chat: '/chat',
    chats: '/chats',
    createChat: '/chat/new',
    editChat: '/chat/edit',
    chatsNotJoined: '/chat/not-joined',
    chatMessage: '/chatMessage',
    acceptChatInvitation: '/chat/invite/accept',
    rejectChatInvitation: '/chat/invite/reject',
    removeChatUser: '/chat/remove/user',

    admin: {
        createArticle: '/admin/article/new',
        editArticle: '/admin/article/edit',

        approveBook: '/book/approve',
        rejectBook: '/book/reject',

        approveAuthor: '/author/approve',
        rejectAuthor: '/author/reject',
    }
}
