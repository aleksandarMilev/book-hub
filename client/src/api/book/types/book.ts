import type { FormikProps } from 'formik';
import type { Dispatch, SetStateAction } from 'react';

import type { AuthorDetails } from '../../author/types/author';
import type { Genre } from '../../genre/types/genre';
import type { ReadingStatus } from '../../readingList/types/readingList';
import type { Review } from '../../review/types/review';

interface BookCore {
  title: string;
  imageUrl?: string | null;
  publishedDate?: string | null;
  shortDescription: string;
  longDescription: string;
}

interface BookRelations {
  authorId?: number | null;
  genres?: NamedEntity[];
}

export type NamedEntity = { id: number; name: string };

export type GenreChip = { id: number | string; name: string };

export type AuthorIntroData = Pick<
  AuthorDetails,
  'id' | 'name' | 'imageUrl' | 'biography' | 'booksCount'
>;

export interface AuthorIntroductionProps {
  author?: AuthorIntroData | null;
}

export type BookFullInfoData = BookCore & {
  id: number;
  authorName?: string | null;
  averageRating?: number | null;
  ratingsCount?: number | null;
  genres: GenreChip[];
  isApproved?: boolean;
  readingStatus?: ReadingStatus;
};

export interface BookFullInfoProps {
  book: BookFullInfoData;
  descriptionPreview: string;
  showFullDescription: boolean;
  setShowFullDescription: Dispatch<SetStateAction<boolean>>;
  isCreator: boolean;
  deleteHandler: () => void;
  id: number | string;
}

export interface UseBookApprovalProps {
  id: number;
  token: string;
  showMessage: (message: string, success?: boolean) => void;
}

export type BookFormProps = {
  bookData?:
    | (Partial<BookCore> &
        BookRelations & {
          id?: number;
        })
    | null;
  isEditMode?: boolean;
};

export type BookFormValues = Required<
  Pick<BookCore, 'title' | 'shortDescription' | 'longDescription'>
> & {
  authorId: number | null;
  imageUrl: string;
  publishedDate: string;
  genres: number[];
};

export type AuthorFormValues = BookFormValues;

export type GenreFormValues = BookFormValues;

export type AuthorSearchProps = {
  authors: NamedEntity[];
  loading: boolean;
  formik: FormikProps<AuthorFormValues>;
};

export type GenreSearchProps = {
  genres: NamedEntity[];
  loading: boolean;
  formik: FormikProps<GenreFormValues>;
  selectedGenres: NamedEntity[];
  setSelectedGenres: Dispatch<SetStateAction<NamedEntity[]>>;
};

export interface BookListItemProps {
  id: number | string;
  imageUrl?: string | null;
  title: string;
  authorName?: string | null;
  shortDescription?: string | null;
  averageRating?: number;
  genres: Genre[];
}

export type LocationState =
  | {
      genreId?: number | string;
      genreName?: string;
      authorId?: number | string;
      authorName?: string;
    }
  | undefined;

export type BookListItemType = {
  id: number | string;
  title: string;
  authorName?: string | null;
  imageUrl?: string | null;
  shortDescription?: string | null;
  averageRating?: number | null;
  genres: Genre[];
};

export type BookDetailsResponse = BookFullInfoData & {
  author: AuthorIntroData;
  creatorId?: number | string;
  reviews?: Review;
  moreThanFiveReviews?: boolean;
};

export type BookUpsertPayload = {
  title: string;
  authorId: number | null;
  imageUrl: string | null;
  publishedDate: string | null;
  shortDescription: string;
  longDescription: string;
  genres: number[];
};
