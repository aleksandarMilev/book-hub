import type { Book } from '@/features/book/types/book.js';

export type Gender = 'male' | 'female' | 'other';

export type Nationality = {
  id: number;
  name: string;
};

export const ALL_NATIONALITIES: Nationality[] = [
  { id: 183, name: 'Unknown' },

  { id: 1, name: 'Afghanistan' },
  { id: 2, name: 'Albania' },
  { id: 3, name: 'Algeria' },
  { id: 4, name: 'Andorra' },
  { id: 5, name: 'Angola' },
  { id: 6, name: 'Antigua and Barbuda' },
  { id: 7, name: 'Argentina' },
  { id: 8, name: 'Armenia' },
  { id: 9, name: 'Australia' },
  { id: 10, name: 'Austria' },
  { id: 11, name: 'Azerbaijan' },
  { id: 12, name: 'Bahamas' },
  { id: 13, name: 'Bahrain' },
  { id: 14, name: 'Bangladesh' },
  { id: 15, name: 'Barbados' },
  { id: 16, name: 'Belarus' },
  { id: 17, name: 'Belgium' },
  { id: 18, name: 'Belize' },
  { id: 19, name: 'Benin' },
  { id: 20, name: 'Bhutan' },
  { id: 21, name: 'Bolivia' },
  { id: 22, name: 'Bosnia and Herzegovina' },
  { id: 23, name: 'Botswana' },
  { id: 24, name: 'Brazil' },
  { id: 25, name: 'Brunei' },
  { id: 26, name: 'Bulgaria' },
  { id: 27, name: 'Burkina Faso' },
  { id: 28, name: 'Burundi' },
  { id: 29, name: 'Cabo Verde' },
  { id: 30, name: 'Cambodia' },
  { id: 31, name: 'Cameroon' },
  { id: 32, name: 'Canada' },
  { id: 33, name: 'Central African Republic' },
  { id: 34, name: 'Chad' },
  { id: 35, name: 'Chile' },
  { id: 36, name: 'China' },
  { id: 37, name: 'Colombia' },
  { id: 38, name: 'Comoros' },
  { id: 39, name: 'Costa Rica' },
  { id: 40, name: 'Croatia' },
  { id: 41, name: 'Cuba' },
  { id: 42, name: 'Cyprus' },
  { id: 43, name: 'Czech Republic' },
  { id: 44, name: 'Denmark' },
  { id: 45, name: 'Djibouti' },
  { id: 46, name: 'Dominica' },
  { id: 47, name: 'Dominican Republic' },
  { id: 48, name: 'Ecuador' },
  { id: 49, name: 'Egypt' },
  { id: 50, name: 'El Salvador' },
  { id: 51, name: 'Equatorial Guinea' },
  { id: 52, name: 'Eritrea' },
  { id: 53, name: 'Estonia' },
  { id: 54, name: 'Eswatini' },
  { id: 55, name: 'Ethiopia' },
  { id: 56, name: 'Fiji' },
  { id: 57, name: 'Finland' },
  { id: 58, name: 'France' },
  { id: 59, name: 'Gabon' },
  { id: 60, name: 'Gambia' },
  { id: 61, name: 'Georgia' },
  { id: 62, name: 'Germany' },
  { id: 63, name: 'Ghana' },
  { id: 64, name: 'Greece' },
  { id: 65, name: 'Grenada' },
  { id: 66, name: 'Guatemala' },
  { id: 67, name: 'Guinea' },
  { id: 68, name: 'Guinea-Bissau' },
  { id: 69, name: 'Guyana' },
  { id: 70, name: 'Haiti' },
  { id: 71, name: 'Honduras' },
  { id: 72, name: 'Hungary' },
  { id: 73, name: 'Iceland' },
  { id: 74, name: 'India' },
  { id: 75, name: 'Indonesia' },
  { id: 76, name: 'Iran' },
  { id: 77, name: 'Iraq' },
  { id: 78, name: 'Ireland' },
  { id: 79, name: 'Israel' },
  { id: 80, name: 'Italy' },
  { id: 81, name: 'Jamaica' },
  { id: 82, name: 'Japan' },
  { id: 83, name: 'Jordan' },
  { id: 84, name: 'Kazakhstan' },
  { id: 85, name: 'Kenya' },
  { id: 86, name: 'Kiribati' },
  { id: 87, name: 'North Korea' },
  { id: 88, name: 'South Korea' },
  { id: 89, name: 'Kuwait' },
  { id: 90, name: 'Kyrgyzstan' },
  { id: 91, name: 'Laos' },
  { id: 92, name: 'Latvia' },
  { id: 93, name: 'Lebanon' },
  { id: 94, name: 'Lesotho' },
  { id: 95, name: 'Liberia' },
  { id: 96, name: 'Libya' },
  { id: 97, name: 'Liechtenstein' },
  { id: 98, name: 'Lithuania' },
  { id: 99, name: 'Luxembourg' },
  { id: 100, name: 'Madagascar' },
  { id: 101, name: 'Malawi' },
  { id: 102, name: 'Malaysia' },
  { id: 103, name: 'Maldives' },
  { id: 104, name: 'Mali' },
  { id: 105, name: 'Malta' },
  { id: 106, name: 'Marshall Islands' },
  { id: 107, name: 'Mauritania' },
  { id: 108, name: 'Mauritius' },
  { id: 109, name: 'Mexico' },
  { id: 110, name: 'Micronesia' },
  { id: 111, name: 'Moldova' },
  { id: 112, name: 'Monaco' },
  { id: 113, name: 'Mongolia' },
  { id: 114, name: 'Montenegro' },
  { id: 115, name: 'Morocco' },
  { id: 116, name: 'Mozambique' },
  { id: 117, name: 'Myanmar' },
  { id: 118, name: 'Namibia' },
  { id: 119, name: 'Nauru' },
  { id: 120, name: 'Nepal' },
  { id: 121, name: 'Netherlands' },
  { id: 122, name: 'New Zealand' },
  { id: 123, name: 'Nicaragua' },
  { id: 124, name: 'Niger' },
  { id: 125, name: 'Nigeria' },
  { id: 126, name: 'North Macedonia' },
  { id: 127, name: 'Norway' },
  { id: 128, name: 'Oman' },
  { id: 129, name: 'Pakistan' },
  { id: 130, name: 'Palau' },
  { id: 131, name: 'Panama' },
  { id: 132, name: 'Papua New Guinea' },
  { id: 133, name: 'Paraguay' },
  { id: 134, name: 'Peru' },
  { id: 135, name: 'Philippines' },
  { id: 136, name: 'Poland' },
  { id: 137, name: 'Portugal' },
  { id: 138, name: 'Qatar' },
  { id: 139, name: 'Romania' },
  { id: 140, name: 'Russia' },
  { id: 141, name: 'Rwanda' },
  { id: 142, name: 'Saint Kitts and Nevis' },
  { id: 143, name: 'Saint Lucia' },
  { id: 144, name: 'Saint Vincent and the Grenadines' },
  { id: 145, name: 'Samoa' },
  { id: 146, name: 'San Marino' },
  { id: 147, name: 'Sao Tome and Principe' },
  { id: 148, name: 'Saudi Arabia' },
  { id: 149, name: 'Senegal' },
  { id: 150, name: 'Serbia' },
  { id: 151, name: 'Seychelles' },
  { id: 152, name: 'Sierra Leone' },
  { id: 153, name: 'Singapore' },
  { id: 154, name: 'Slovakia' },
  { id: 155, name: 'Slovenia' },
  { id: 156, name: 'Solomon Islands' },
  { id: 157, name: 'Somalia' },
  { id: 158, name: 'South Africa' },
  { id: 159, name: 'South Sudan' },
  { id: 160, name: 'Spain' },
  { id: 161, name: 'Sri Lanka' },
  { id: 162, name: 'Sudan' },
  { id: 163, name: 'Suriname' },
  { id: 164, name: 'Sweden' },
  { id: 165, name: 'Switzerland' },
  { id: 166, name: 'Syria' },
  { id: 167, name: 'Taiwan' },
  { id: 168, name: 'Tajikistan' },
  { id: 169, name: 'Tanzania' },
  { id: 170, name: 'Thailand' },
  { id: 171, name: 'Togo' },
  { id: 172, name: 'Tonga' },
  { id: 173, name: 'Trinidad and Tobago' },
  { id: 174, name: 'Tunisia' },
  { id: 175, name: 'Turkey' },
  { id: 176, name: 'Turkmenistan' },
  { id: 177, name: 'Tuvalu' },
  { id: 178, name: 'Uganda' },
  { id: 179, name: 'Ukraine' },
  { id: 180, name: 'United Arab Emirates' },
  { id: 181, name: 'United Kingdom' },
  { id: 182, name: 'United States' },
  { id: 184, name: 'Uruguay' },
  { id: 185, name: 'Uzbekistan' },
  { id: 186, name: 'Vanuatu' },
  { id: 187, name: 'Vatican City' },
  { id: 188, name: 'Venezuela' },
  { id: 189, name: 'Vietnam' },
  { id: 190, name: 'Yemen' },
  { id: 191, name: 'Zambia' },
  { id: 192, name: 'Zimbabwe' },
];

export const getNationalityName = (id?: number | null) => {
  if (id == null) {
    return 'Nationality unknown';
  }

  const match = ALL_NATIONALITIES.find((n) => n.id === id);
  return match?.name ?? 'Nationality unknown';
};

export const GenderToServer: Record<Gender, number> = {
  male: 0,
  female: 1,
  other: 2,
};

export const genderFromServer = (value: number): Gender => {
  switch (value) {
    case 0:
      return 'male';
    case 1:
      return 'female';
    case 2:
    default:
      return 'other';
  }
};

export type Author = {
  id: string;
  name: string;
  imagePath: string;
  biography: string;
  booksCount: number;
  averageRating: number;
};

export type AuthorDetailsDto = Omit<AuthorDetails, 'gender'> & {
  gender: number;
};

export type AuthorDetails = Author & {
  penName?: string | null;
  nationality: number;
  gender: Gender;
  bornAt?: string | null;
  diedAt?: string | null;
  creatorId?: string | null;
  isApproved: boolean;
  topBooks: Book[];
};

export type AuthorNames = Pick<Author, 'id' | 'name'>;

export type CreateAuthor = {
  name: string;
  image?: File | null;
  biography: string;
  penName?: string | null;
  nationality: number | null;
  gender: Gender;
  bornAt?: string | null | undefined;
  diedAt?: string | null | undefined;
};
