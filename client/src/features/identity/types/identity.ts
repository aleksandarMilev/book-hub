export interface LoginResponse {
  token: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  socialMediaUrl?: string | null;
  biography?: string | null;
  isPrivate: boolean;
  image?: File | null;
}

export interface LoginRequest {
  credentials: string;
  rememberMe: boolean;
  password: string;
}

export type DecodedToken = {
  nameid: string;
  unique_name: string;
  email: string;
  role?: string;
};

export interface RegisterFormValues extends RegisterRequest {
  confirmPassword: string;
}
