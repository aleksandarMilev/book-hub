export const baseUrl = 'https://localhost:7216';

export const httpCodes = {
    get: 'GET',
    post: 'POST',
    put: 'PUT',
    delete: 'DELETE',
};

export const apiRoutes = {
    home: '/',
    register: '/identity/register',
    login: '/identity/login',
    books: '/books',
    createBook: '/books/new',
    bookDetails: '/books/:id',
};
