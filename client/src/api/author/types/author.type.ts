export interface AuthorBase {
  id: number;
  name: string;
  penName?: string | null;
  imageUrl?: string | null;
  bornAt?: string | null;
  diedAt?: string | null;
  gender: string;
  nationality?: {
    id: number | string;
    name: string;
  } | null;
  biography: string;
}

export type Author = AuthorBase;

export interface AuthorTopBook {
  id: number | string;
  title: string;
  imageUrl?: string | null;
  authorName?: string | null;
  shortDescription?: string | null;
  averageRating?: number;
  genres: {
    id: number | string;
    name: string;
  }[];
}

export type AuthorDetails = Author & {
  creatorId?: number | string;
  isApproved?: boolean;
  averageRating?: number;
  booksCount?: number;
  topBooks?: AuthorTopBook[];
};

export type AuthorInput = {
  name: string;
  penName?: string | null;
  imageUrl?: string | null;
  bornAt?: string | null;
  diedAt?: string | null;
  gender: string;
  nationality?: number | string | null;
  biography: string;
};
