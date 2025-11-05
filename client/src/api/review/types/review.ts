export interface Review {
  id: number;
  bookId: number;
  content: string;
  rating: number;
  creatorId: number;
  createdBy: string;
  upvotes: number;
  downvotes: number;
  createdOn?: string;
  updatedOn?: string;
}

export interface ReviewInput {
  bookId: number;
  content: string;
  rating: number;
}
