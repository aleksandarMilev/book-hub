export interface LoginResponse {
  token: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
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
