import type { GenreName } from './genreName';

export interface GenreDetails extends GenreName {
  description: string;
  imageUrl: string;
  topBooks: Array<{
    id: number;
    title: string;
    imageUrl: string;
    averageRating: number;
  }>;
}
