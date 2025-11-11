import type { Book } from '@/features/book/book';
import type { Nationality } from '@/features/nationality/types/nationality';

export type Author = {
  id: number;
  name: string;
  imageUrl: string;
  biography: string;
  booksCount: number;
  averageRating: number;
};

export type AuthorDetails = Author & {
  penName?: string;
  nationality: Nationality;
  gender: string;
  bornAt?: string;
  diedAt?: string;
  creatorId?: string;
  isApproved: boolean;
  topBooks: Book[];
};

export type AuthorNames = Pick<Author, 'id' | 'name'>;

export type CreateAuthor = {
  name: string;
  imageUrl?: string;
  biography: string;
  penName?: string;
  nationalityId?: string;
  gender: string;
  bornAt?: string;
  diedAt?: string;
};
