import type { BookListItemType } from '../../book/types/book';

export interface Genre {
  id: number;
  name: string;
}

export interface GenreDetails extends Genre {
  description?: string | null;
  imageUrl?: string | null;
  topBooks?: BookListItemType[];
}
