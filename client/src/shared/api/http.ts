import axios, { type AxiosRequestConfig, HttpStatusCode } from 'axios';

import { baseAdminUrl, baseUrl, routes } from '@/shared/lib/constants/api';
import { IsCanceledError } from '@/shared/lib/utils/utils';

export const http = axios.create({ baseURL: baseUrl });
export const httpAdmin = axios.create({ baseURL: baseAdminUrl });

// Identity endpoints never count as an expired session, so a failed login can't trigger a redirect.
const identityEndpoints = [
  routes.login,
  routes.register,
  routes.forgotPassword,
  routes.resetPassword,
];

// A 401 for a request that carried a bearer token: the token is expired or no longer valid.
export const isSessionExpiredError = (error: unknown): boolean => {
  if (!axios.isAxiosError(error) || error.response?.status !== HttpStatusCode.Unauthorized) {
    return false;
  }

  const url = error.config?.url ?? '';
  if (identityEndpoints.some((endpoint) => url.endsWith(endpoint))) {
    return false;
  }

  const authorization = error.config?.headers.get('Authorization');

  return typeof authorization === 'string' && authorization.trim().length > 'Bearer'.length;
};

// Calls `onExpired` on every session-expired 401 from `http` and `httpAdmin`, then rethrows the
// error so callers still go through processError. Returns a function that removes the interceptors.
export const onSessionExpired = (onExpired: () => void) => {
  const rejected = (error: unknown) => {
    if (isSessionExpiredError(error)) {
      onExpired();
    }

    return Promise.reject(error);
  };

  const registrations = [http, httpAdmin].map(
    (instance) => [instance, instance.interceptors.response.use(undefined, rejected)] as const,
  );

  return () => {
    registrations.forEach(([instance, id]) => instance.interceptors.response.eject(id));
  };
};

export const getAuthConfig = (token: string, signal?: AbortSignal): AxiosRequestConfig => {
  const config: AxiosRequestConfig = {
    // No Content-Type here: axios picks JSON for plain objects and multipart (with boundary)
    // for FormData. Forcing JSON made axios serialize FormData to JSON and drop files.
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  if (signal) {
    config.signal = signal;
  }

  return config;
};

export const getPublicConfig = (signal?: AbortSignal): AxiosRequestConfig => {
  const config: AxiosRequestConfig = {};

  if (signal) {
    config.signal = signal;
  }

  return config;
};

export const processError = (error: unknown, fallbackMessage: string): never => {
  const isRequestCanceled = axios.isCancel?.(error) || IsCanceledError(error);

  if (isRequestCanceled) {
    throw error;
  }

  if (axios.isAxiosError(error) && error.response?.data) {
    const data = error.response.data;
    const serverMessage = data.errorMessage || data.message || data.title;

    if (serverMessage && typeof serverMessage === 'string') {
      throw new Error(serverMessage);
    }
  }

  throw new Error(fallbackMessage);
};
