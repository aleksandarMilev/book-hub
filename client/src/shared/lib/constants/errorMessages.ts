export const baseErrors = {
  general: 'Something went wrong. Please refresh the page or try again later!',
  create:
    "Oops! We couldn't create your {entity}. Please try again later or contact support if the issue persists.",
  edit: "Oops! We couldn't edit your {entity}. Please try again later or contact support if the issue persists.",
  delete:
    "Oops! We couldn't delete your {entity}. Please try again later or contact support if the issue persists.",
  notFound:
    "We couldn't find the {entity} you're looking for. Please check the URL or try again later.",
  namesBadRequest:
    "Apologies, we're experiencing issues and can't display {entity} names at the moment. Please try again later or contact us for assistance.",
  topThree:
    'Something went wrong while loading our top {entityPlural} for you. Please refresh the page or try again later!',
  approve:
    'Something went wrong while approving this {entity}. Please refresh the page or try again later!',
  reject:
    'Something went wrong while rejecting this {entity}. Please refresh the page or try again later!',
};

export const errors = {
  statistics: {
    get: 'Something went wrong while loading our statistics. Please refresh the page or try again later!',
  },
  readingList: {
    currentlyReading:
      "Apologies, we're experiencing issues and can't display the user currently reading books at the moment. Please try again later or contact us for assistance.",
    add: 'Something went wrong while adding the book to your list. Please refresh the page or try again later!',
    remove:
      'Something went wrong while removing the book from your list. Please refresh the page or try again later!',
  },
  notification: {
    markAsRead: baseErrors.general,
    delete:
      "Oops! We couldn't delete this notification. Please try again later or contact support if the issue persists.",
    all: 'Something went wrong while loading your notifications. Please refresh the page or try again later!',
    lastThree:
      "Apologies, we're experiencing issues and can't display your notifications at the moment. Please try again later or contact us for assistance.",
  },
  profile: {
    names: baseErrors.namesBadRequest.replace('{entity}', 'user'),
    topThree: baseErrors.topThree.replace('{entityPlural}', 'users'),
    get: 'Something went wrong while loading your profile. Please refresh the page or try again later!',
    getOther:
      'Something went wrong while loading this profile. Please refresh the page or try again later!',
    create: baseErrors.create.replace('{entity}', 'profile'),
    edit: baseErrors.edit.replace('{entity}', 'profile'),
    delete: baseErrors.delete.replace('{entity}', 'profile'),
  },
  article: {
    get: 'Something went wrong while loading this article. Please refresh the page or try again later!',
    create: baseErrors.create.replace('{entity}', 'article'),
    edit: baseErrors.edit.replace('{entity}', 'article'),
    delete: baseErrors.delete.replace('{entity}', 'article'),
    notFound: baseErrors.notFound.replace('{entity}', 'article'),
  },
  author: {
    approve: baseErrors.approve.replace('{entity}', 'author'),
    reject: baseErrors.reject.replace('{entity}', 'author'),
    topThree: baseErrors.topThree.replace('{entityPlural}', 'authors'),
    namesBadRequest: baseErrors.namesBadRequest.replace('{entity}', 'author'),
    notFound: baseErrors.notFound.replace('{entity}', 'author'),
    create:
      "We're sorry, something went wrong while creating the author. Please try again later. If the issue persists, contact support for assistance.",
    edit: "We're sorry, something went wrong while updating the author's details. Please try again later. If the issue persists, contact support for assistance.",
    delete: baseErrors.delete.replace('{entity}', 'author'),
  },
  genre: {
    details: baseErrors.notFound.replace('{entity}', 'genre'),
    namesBadRequest: baseErrors.namesBadRequest.replace('{entity}', 'genre'),
  },
  identity: {
    register: 'Something went wrong while creating your account. Please try again later.',
    login: 'Login failed. Please check your username and password and try again.',
  },
  nationality: {
    namesBadRequest: baseErrors.namesBadRequest.replace('{entity}', 'nationality'),
  },
  search: {
    badRequest:
      "We couldn't complete your search. Please try again, refresh the page, or check back later. If the issue persists, contact support. Thank you for your patience!",
    topThree: baseErrors.topThree.replace('{entityPlural}', 'picks'),
  },
  book: {
    approve: baseErrors.approve.replace('{entity}', 'book'),
    reject: baseErrors.reject.replace('{entity}', 'book'),
    topThree: baseErrors.topThree.replace('{entityPlural}', 'books'),
    notFound: baseErrors.notFound.replace('{entity}', 'book'),
    create: baseErrors.create.replace('{entity}', 'book'),
    edit: baseErrors.edit.replace('{entity}', 'book'),
    delete: baseErrors.delete.replace('{entity}', 'book'),
  },
  chat: {
    removeUser:
      'Something went wrong while removing this user from the chat. Please try again later.',
    accept: 'Something went wrong while adding you in this chat. Please try again later.',
    reject: 'Something went wrong while removing you from this chat. Please try again later.',
    createMessage: 'Something went wrong sending your message. Please try again later.',
    editMessage: 'Something went wrong editing your message. Please try again later.',
    deleteMessage: 'Something went wrong deleting your message. Please try again later.',
    details: 'Something went wrong while loading this chat. Please try again later.',
    names:
      'Something went wrong while loading your chats. Please refresh the page or try again later!',
    create: baseErrors.create.replace('{entity}', 'chat'),
    edit: baseErrors.edit.replace('{entity}', 'chat'),
    delete: baseErrors.delete.replace('{entity}', 'chat'),
    addUser:
      'Something went wrong while adding this user to the chat. Please refresh the page or try again later!',
  },
  review: {
    create: baseErrors.create.replace('{entity}', 'review'),
    edit: baseErrors.edit.replace('{entity}', 'review'),
    delete: baseErrors.delete.replace('{entity}', 'review'),
    vote: 'An error occurred while processing your vote. Please try again later.',
    list: 'Failed to load reviews.',
  },
};
