export interface AuthorSearchResult {
  id: number;
  name: string;
  penName?: string | null;
  imageUrl?: string | null;
  averageRating?: number;
  booksCount?: number;
  nationality?: {
    id: number | string;
    name: string;
  } | null;
}
