import type { Genre } from '../../genre/types/genre';

export type ReadingStatus = 'read' | 'toRead' | 'currentlyReading' | null;

export interface ReadingListItem {
  id: number;
  title: string;
  genres: Genre[];
}
