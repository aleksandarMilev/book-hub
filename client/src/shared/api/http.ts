import axios, { type AxiosRequestConfig } from 'axios';

import { baseAdminUrl, baseUrl } from '@/shared/lib/constants/api';

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

export function processError(error: unknown, message: string): never {
  const isRequestCanceled =
    axios.isCancel?.(error) || (error instanceof Error && error.name === 'CanceledError');

  if (isRequestCanceled) {
    throw error;
  }

  throw new Error(message);
}
