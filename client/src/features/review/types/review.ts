export type Review = {
  id: number;
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
  bookId?: string | undefined;
};

export type VoteRequest = {
  reviewId: number;
  isUpvote: boolean;
};
