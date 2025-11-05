export type DecodedToken = {
  nameid: string;
  unique_name: string;
  email: string;
  role?: string;
};

export interface LoginRequest {
  credentials: string;
  password: string;
  rememberMe: boolean;
}

export interface LoginResponse {
  token: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface LoginFormValues {
  credentials: string;
  password: string;
  rememberMe: boolean;
}

export const loginInitialValues: LoginFormValues = {
  credentials: '',
  password: '',
  rememberMe: false,
};

export interface RegisterFormValues {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export const registerInitialValues: RegisterFormValues = {
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
};
