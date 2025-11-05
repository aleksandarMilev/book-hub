import type { BookListItemType } from '../../book/types/book';
import type { Genre } from '../../genre/types/genre';

export type BookSearchResult = Omit<BookListItemType, 'genres'> & {
  genres?: Genre[];
};
