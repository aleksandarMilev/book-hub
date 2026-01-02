export type PrivateProfile = {
  id: string;
  firstName: string;
  lastName: string;
  imagePath: string;
  isPrivate: boolean;
};

export type Profile = PrivateProfile & {
  dateOfBirth?: string | null;
  socialMediaUrl?: string | null;
  biography?: string | null;
  createdBooksCount: number;
  createdAuthorsCount: number;
  reviewsCount: number;
  readBooksCount: number;
  toReadBooksCount: number;
  currentlyReadingBooksCount: number;
};

export type CreateProfile = {
  firstName: string;
  lastName: string;
  dateOfBirth?: string | null;
  socialMediaUrl?: string | null;
  biography?: string | null;
  isPrivate: boolean;
  image?: File | null;
  removeImage: boolean;
};
