import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from '@/features/identity/types/identity';
import { http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export const register = async (
  username: string,
  email: string,
  password: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.register}`;
    const payload: RegisterRequest = { username, email, password };
    const response = await http.post<LoginResponse>(url, payload, getConfig(signal));

    return response.data;
  } catch (error) {
    processError(error, errors.identity.create);
  }
};

export const login = async (
  credentials: string,
  password: string,
  rememberMe: boolean,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.login}`;
    const payload: LoginRequest = { credentials, password, rememberMe };
    const response = await http.post<LoginResponse>(url, payload, getConfig(signal));

    return response.data;
  } catch (error) {
    processError(error, errors.identity.create);
  }
};

const getConfig = (signal?: AbortSignal) => {
  return {
    headers: { 'Content-Type': 'application/json' },
    ...(signal ? { signal } : {}),
  };
};
