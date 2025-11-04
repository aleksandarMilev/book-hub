export interface Author {
  id?: number;
  name: string;
  penName?: string | null;
  imageUrl?: string | null;
  gender?: string;
  biography?: string;
  nationalityId?: string | null;
  nationality?: { id: string; name: string };
  bornAt?: string | null;
  diedAt?: string | null;
  creatorId?: string;
  isApproved?: boolean;
  averageRating?: number;
  booksCount?: number;
  topBooks?: Array<{
    id: string;
    title: string;
    imageUrl?: string | null;
    averageRating?: number;
  }>;
}
