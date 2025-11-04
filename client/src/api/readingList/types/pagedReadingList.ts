import type { ReadingListItem } from './readingListItem';

export interface PagedReadingList {
  items: ReadingListItem[];
  totalItems: number;
}
