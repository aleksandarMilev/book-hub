import type { LoginRequest, LoginResponse, RegisterRequest } from './types/identity';

import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';

import { http } from '../common/http';
import { returnIfRequestCanceled } from '../common/utils';


export async function register(
  username: string,
  email: string,
  password: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.register}`;
    const payload: RegisterRequest = { username, email, password };

    const config = {
      headers: { 'Content-Type': 'application/json' },
      ...(signal ? { signal } : {}),
    };

    const response = await http.post<LoginResponse>(url, payload, config);

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.identity.register);
    throw error;
  }
}

export async function login(
  credentials: string,
  password: string,
  rememberMe: boolean,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.login}`;
    const payload: LoginRequest = { credentials, password, rememberMe };

    const config = {
      headers: { 'Content-Type': 'application/json' },
      ...(signal ? { signal } : {}),
    };

    const response = await http.post<LoginResponse>(url, payload, config);

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.identity.login);
    throw error;
  }
}
