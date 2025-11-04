export interface Book {
  id?: number;
  title: string;
  shortDescription: string;
  longDescription: string;
  imageUrl?: string | null;
  publishedDate?: string | null;
  averageRating?: number;
  ratingsCount?: number;
  authorId?: string | null;
  authorName?: string | null;
  creatorId?: string;
  isApproved?: boolean;
  genres?: { id: string; name: string }[];
  reviews?: Array<{
    id: string;
    content: string;
    creatorId: string;
    rating: number;
  }>;
  moreThanFiveReviews?: boolean;
  readingStatus?: string | null;
}
