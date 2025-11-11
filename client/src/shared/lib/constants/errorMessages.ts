const createEntityErrors = (entity: string, allMessage?: string) => ({
  all:
    allMessage ??
    `Something went wrong while loading ${entity}s. Please refresh the page or try again later!`,
  byId: baseErrors.notFound.replace('{entity}', entity),
  create: baseErrors.create.replace('{entity}', entity),
  edit: baseErrors.edit.replace('{entity}', entity),
  delete: baseErrors.delete.replace('{entity}', entity),
});

export const baseErrors = {
  general: 'Something went wrong. Please refresh the page or try again later!',
  create:
    "Oops! We couldn't create your {entity}. Please try again later or contact support if the issue persists.",
  edit: "Oops! We couldn't edit your {entity}. Please try again later or contact support if the issue persists.",
  delete:
    "Oops! We couldn't delete your {entity}. Please try again later or contact support if the issue persists.",
  notFound:
    "We couldn't find the {entity} you're looking for. Please check the URL or try again later.",
};

export const errors = {
  statistics: createEntityErrors('statistic'),
  readingList: {
    ...createEntityErrors(
      'reading list item',
      'Something went wrong while loading your reading list. Please refresh the page or try again later!',
    ),
    add: 'Something went wrong while adding this book to your reading list.',
    remove: 'Something went wrong while removing this book from your reading list.',
  },
  notification: {
    ...createEntityErrors('notification'),
    markAsRead: baseErrors.general,
    lastThree:
      'Something went wrong while loading your last notifications. Please refresh the page or try again later!',
  },
  profile: {
    ...createEntityErrors('profile'),
    topThree: "We couldn't load top users.",
  },
  article: createEntityErrors('article'),
  author: {
    ...createEntityErrors('author'),
    topThree: "We couldn't load top authors.",
  },
  identity: createEntityErrors('account'),
  nationality: createEntityErrors('nationality'),
  search: createEntityErrors(
    'search result',
    'Something went wrong while loading search results. Please refresh the page or try again later!',
  ),
  book: createEntityErrors('book'),
  chat: createEntityErrors('chat'),
  review: {
    ...createEntityErrors('review'),
    vote: {
      up: 'Something went wrong will processing your upvote',
      down: 'Something went wrong will processing your downvote',
    },
  },
};
