import axios, { type AxiosRequestConfig } from 'axios';

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

export function returnIfRequestCanceled(error: unknown, message: string): void {
  const isRequestCanceled =
    axios.isCancel?.(error) || (error instanceof Error && error.name === 'CanceledError');

  if (isRequestCanceled) {
    throw error;
  }

  throw new Error(message);
}
