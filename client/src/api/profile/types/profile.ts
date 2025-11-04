import type { BaseProfile } from './baseProfile';

export interface Profile extends BaseProfile {
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
}
