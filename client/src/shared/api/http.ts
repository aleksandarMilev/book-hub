import axios, { type AxiosRequestConfig } from 'axios';

import { baseAdminUrl, baseUrl } from '@/shared/lib/constants/api.js';

export const http = axios.create({ baseURL: baseUrl });
export const httpAdmin = axios.create({ baseURL: baseAdminUrl });

export function getAuthConfig(token: string, signal?: AbortSignal): AxiosRequestConfig {
  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  };

  if (signal) {
    config.signal = signal;
  }

  return config;
}

export const getAuthConfigForFile = (token: string, signal?: AbortSignal) => {
  const config = getAuthConfig(token, signal);
  const fileConfig = {
    ...config,
    headers: {
      ...(config.headers ?? {}),
      'Content-Type': 'multipart/form-data',
    },
  };

  return fileConfig;
};

export function getPublicConfig(signal?: AbortSignal): AxiosRequestConfig {
  const config: AxiosRequestConfig = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  if (signal) {
    config.signal = signal;
  }

  return config;
}

export function processError(error: unknown, message: string): never {
  const isRequestCanceled =
    axios.isCancel?.(error) || (error instanceof Error && error.name === 'CanceledError');

  if (isRequestCanceled) {
    throw error;
  }

  throw new Error(message);
}
