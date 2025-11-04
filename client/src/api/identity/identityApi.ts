import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import type { LoginResponse } from './types/loginResponse';
import type { RegisterRequest } from './types/registerRequest';
import type { LoginRequest } from './types/loginRequest';

export async function register(username: string, email: string, password: string) {
  try {
    const url = `${baseUrl}${routes.register}`;
    const payload: RegisterRequest = { username, email, password };
    const response = await axios.post<LoginResponse>(url, payload, {
      headers: { 'Content-Type': 'application/json' },
    });

    return response.data;
  } catch (error: any) {
    const message = error?.response?.data?.errorMessage || errors.identity.register;
    throw new Error(message);
  }
}

export async function login(credentials: string, password: string, rememberMe: boolean) {
  try {
    const url = `${baseUrl}${routes.login}`;
    const payload: LoginRequest = { credentials, password, rememberMe };
    const response = await axios.post<LoginResponse>(url, payload, {
      headers: { 'Content-Type': 'application/json' },
    });

    return response.data;
  } catch (error: any) {
    const message = error?.response?.data?.errorMessage || errors.identity.login;
    throw new Error(message);
  }
}
