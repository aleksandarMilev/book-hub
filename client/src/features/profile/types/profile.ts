export type PrivateProfile = {
  id: string;
  firstName: string;
  lastName: string;
  imageUrl: string;
  isPrivate: boolean;
};

export type Profile = PrivateProfile & {
  phoneNumber: string;
  dateOfBirth: string;
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
  imageUrl: string;
  phoneNumber: string;
  dateOfBirth: string;
  socialMediaUrl?: string | null;
  biography?: string | null;
  isPrivate: boolean;
};
