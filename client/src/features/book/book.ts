import type { GenreName } from '@/features/genre/types/genre';

export interface Book {
  id: number;
  title: string;
  authorName?: string | null;
  imageUrl: string;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
}
