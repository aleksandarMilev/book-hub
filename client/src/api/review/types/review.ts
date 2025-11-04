import type { CreateReview } from './createReview';

export interface Review extends CreateReview {
  id: number;
  userId: string;
  username: string;
  createdOn: string;
  modifiedOn?: string | null;
  upvotes: number;
  downvotes: number;
}
