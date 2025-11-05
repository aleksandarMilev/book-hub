import type { FormikProps } from 'formik';

interface AuthorCore {
  name: string;
  penName?: string | null;
  imageUrl?: string | null;
  biography: string;
}

interface AuthorLife {
  bornAt?: string | null;
  diedAt?: string | null;
  gender: Gender | string;
}

interface AuthorMetadata {
  id: number;
  nationality?: NationalityRef | null;
}

interface AuthorInputExtras {
  nationality?: number | string | null;
}

export type Gender = 'male' | 'female' | 'other';

export interface NationalityRef {
  id: number;
  name: string;
}

export interface GenreRef {
  id: number | string;
  name: string;
}

export interface AuthorBase extends AuthorCore, AuthorLife, AuthorMetadata {}

export type Author = AuthorBase;

export interface AuthorName
  extends Pick<AuthorCore, 'name' | 'penName' | 'imageUrl'>,
    Pick<AuthorMetadata, 'id'> {}

export interface AuthorTopBook {
  id: number | string;
  title: string;
  imageUrl?: string | null;
  authorName?: string | null;
  shortDescription?: string | null;
  averageRating?: number;
  genres: GenreRef[];
}

export type AuthorDetails = Author & {
  creatorId?: number | string;
  isApproved?: boolean;
  averageRating?: number;
  booksCount?: number;
  topBooks?: AuthorTopBook[];
};

export interface AuthorInput extends AuthorCore, AuthorLife, AuthorInputExtras {}

export interface UseAuthorApprovalProps {
  authorId: number;
  authorName: string;
  token: string;
  onSuccess: (message: string, success?: boolean) => void;
}

export interface ApproveRejectButtonsProps {
  authorId: number;
  authorName: string;
  initialIsApproved: boolean;
  token: string;
  onSuccess: (message: string, success?: boolean) => void;
}

export interface AuthorFormValues extends Omit<AuthorInput, 'nationality'> {
  nationality: number | null;
  nationalityName: string;
}

export interface AuthorFormProps {
  authorData?: Author | null;
  isEditMode?: boolean;
}

export interface GenderRadioValues {
  gender: string;
}

export interface GenderRadioProps {
  formik: FormikProps<GenderRadioValues>;
}

export interface NationalitySearchValues {
  nationality: number | string;
}

export interface NationalitySearchProps {
  nationalities: NationalityRef[];
  loading: boolean;
  formik: FormikProps<NationalitySearchValues>;
}
