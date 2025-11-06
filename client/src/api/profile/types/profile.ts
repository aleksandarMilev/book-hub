interface BaseProfileFields {
  firstName: string;
  lastName: string;
  imageUrl: string | null;
  isPrivate: boolean;
}

interface ProfileContactFields {
  phoneNumber: string;
  dateOfBirth: string;
  socialMediaUrl: string | null;
  biography: string | null;
}

interface ProfileStatsFields {
  createdBooksCount: number;
  createdAuthorsCount: number;
  reviewsCount: number;
  currentlyReadingBooksCount: number;
  toReadBooksCount: number;
  readBooksCount: number;
}

export interface ProfileSummary extends BaseProfileFields {
  id: string;
}

export interface ProfileStats extends ProfileStatsFields {}

export interface Profile
  extends ProfileSummary,
    Partial<ProfileStatsFields>,
    ProfileContactFields {}

export interface ProfileInput extends BaseProfileFields, ProfileContactFields {}

export interface ProfileListItemProps
  extends Pick<BaseProfileFields, 'imageUrl' | 'firstName' | 'lastName' | 'isPrivate'> {
  id: string;
}
