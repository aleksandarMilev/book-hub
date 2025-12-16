import type { Author } from '@/features/author/types/author.js';
import type { GenreName } from '@/features/genre/types/genre.js';
import type { ReadingStatusAPI } from '@/features/reading-list/types/readingList.js';
import type { Review } from '@/features/review/types/review.js';

export interface Book {
  id: string;
  title: string;
  authorName?: string | null;
  imagePath: string;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
}

export interface CreateBook {
  title: string;
  authorId?: string | null;
  image?: File | null;
  shortDescription: string;
  longDescription: string;
  publishedDate?: string | null | undefined;
  genres: string[];
}

export interface BookDetails extends Book {
  publishedDate?: string | null;
  ratingsCount: number;
  longDescription: string;
  creatorId?: string | null;
  moreThanFiveReviews: boolean;
  isApproved: boolean;
  readingStatus?: ReadingStatusAPI | null;
  author?: Author | null;
  reviews: Review[];
}
