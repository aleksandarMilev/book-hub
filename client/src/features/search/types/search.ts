import type { GenreName } from '@/features/genre/types/genre.js';

export type ArticlesSearchResult = {
  id: number;
  title: string;
  introduction: string;
  imageUrl: string;
  createdOn: string;
};

export type AuthorsSearchResult = {
  id: number;
  name: string;
  penName?: string | null;
  imageUrl: string;
  averageRating: number;
};

export type BooksSearchResult = {
  id: number;
  title: string;
  authorName?: string | null;
  imageUrl: string;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
};

export type ChatsSearchResult = {
  id: number;
  name: string;
  imageUrl: string;
  creatorId: string;
};

export type ProfilesSearchResult = {
  id: string;
  firstName: string;
  lastName: string;
  imageUrl: string;
  isPrivate: boolean;
};
