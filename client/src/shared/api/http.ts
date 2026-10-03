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

const isNonBlankString = (value: unknown): value is string =>
  typeof value === 'string' && value.trim() !== '';

// Server errors are RFC 9457 ProblemDetails. `detail` holds the human-readable message, and a
// validation error (ValidationProblemDetails) holds per-field messages in `errors`. The generic
// `title` ("Bad Request", "Not Found") is never shown: the caller's fallback is more useful.
export const getProblemMessage = (data: unknown): string | null => {
  if (typeof data !== 'object' || data === null) {
    return null;
  }

  const { detail, errors } = data as { detail?: unknown; errors?: unknown };

  if (isNonBlankString(detail)) {
    return detail;
  }

  if (typeof errors === 'object' && errors !== null) {
    for (const messages of Object.values(errors)) {
      const first: unknown = Array.isArray(messages) ? messages.find(isNonBlankString) : undefined;

      if (isNonBlankString(first)) {
        return first;
      }
    }
  }

  return null;
};

export const processError = (error: unknown, fallbackMessage: string): never => {
  const isRequestCanceled = axios.isCancel?.(error) || IsCanceledError(error);

  if (isRequestCanceled) {
    throw error;
  }

  // A 5xx detail is generic ("An unexpected error occurred."), so the caller's message is better.
  const status = axios.isAxiosError(error) ? error.response?.status : undefined;
  if (axios.isAxiosError(error) && status !== undefined && status < 500) {
    const serverMessage = getProblemMessage(error.response?.data);

    if (serverMessage !== null) {
      throw new Error(serverMessage);
    }
  }

  throw new Error(fallbackMessage);
};
