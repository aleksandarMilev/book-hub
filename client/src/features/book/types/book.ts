import type { Author } from '@/features/author/types/author.js';
import type { GenreName } from '@/features/genre/types/genre.js';
import type { Review } from '@/features/review/types/review.js';

export interface Book {
  id: number;
  title: string;
  authorName?: string | null;
  imageUrl: string;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
}

export interface CreateBook {
  title: string;
  authorId?: number | null;
  imageUrl?: string | null;
  shortDescription: string;
  longDescription: string;
  publishedDate?: string | null;
  genres: number[];
}

export interface BookDetails extends Book {
  publishedDate?: string | null;
  ratingsCount: number;
  longDescription: string;
  creatorId?: string | null;
  moreThanFiveReviews: boolean;
  isApproved: boolean;
  readingStatus?: string | null;
  author?: Author | null;
  reviews: Review[];
}
