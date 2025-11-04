export interface CreateProfile {
  firstName: string;
  lastName: string;
  imageUrl: string | null;
  phoneNumber: string;
  dateOfBirth: string;
  socialMediaUrl?: string | null;
  biography?: string | null;
  isPrivate: boolean;
}
