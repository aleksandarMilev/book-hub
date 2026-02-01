import type { Book } from '@/features/book/types/book';

export interface GenreName {
  id: string;
  name: string;
}

export interface GenreDetails extends GenreName {
  description: string;
  imagePath: string;
  topBooks: Book[];
}


