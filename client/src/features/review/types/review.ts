export type Review = {
  id: string;
  content: string;
  rating: number;
  upvotes: number;
  downvotes: number;
  creatorId: string;
  createdBy: string;
  createdOn: string;
  modifiedOn?: string | null;
  bookId: string;
};

export type CreateReview = {
  content: string;
  rating: number;
  bookId?: string;
};

export type VoteRequest = {
  reviewId: string;
  isUpvote: boolean;
};
