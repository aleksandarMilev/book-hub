import type { Book } from '@/features/book/types/book.js';

export interface GenreName {
  id: number;
  name: string;
}

export interface GenreDetails extends GenreName {
  description: string;
  imageUrl: string;
  topBooks: Book[];
}
