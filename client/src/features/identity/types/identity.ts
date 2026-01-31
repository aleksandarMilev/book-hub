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
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
};

export interface RegisterFormValues extends RegisterRequest {
  confirmPassword: string;
}

