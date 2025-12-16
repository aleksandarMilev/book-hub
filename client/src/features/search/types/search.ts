import type { GenreName } from '@/features/genre/types/genre.js';

export type GenresSearchResult = {
  id: string;
  name: string;
  imagePath: string;
};

export type ArticlesSearchResult = {
  id: string;
  title: string;
  introduction: string;
  imagePath: string;
  createdOn: string;
};

export type AuthorsSearchResult = {
  id: string;
  name: string;
  penName?: string | null;
  imagePath: string;
  averageRating: number;
};

export type BooksSearchResult = {
  id: string;
  title: string;
  authorName?: string | null;
  imagePath: string;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
};

export type ChatsSearchResult = {
  id: string;
  name: string;
  imagePath: string;
  creatorId: string;
};

export type ProfilesSearchResult = {
  id: string;
  firstName: string;
  lastName: string;
  imageUrl: string;
  isPrivate: boolean;
};
