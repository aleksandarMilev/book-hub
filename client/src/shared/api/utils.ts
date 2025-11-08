import axios, { AxiosError, type AxiosRequestConfig, HttpStatusCode } from 'axios';

import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { HttpError } from '@/shared/types/errors/httpError';

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

export function handleRequestError(error: unknown, message: string): never {
  const isRequestCanceled = axios.isCancel?.(error) || (IsError(error) && IsCanceledError(error));
  if (isRequestCanceled) {
    throw error;
  }

  const axiosError = error as AxiosError;
  throw HttpError.with()
    .message(message)
    .andName('Article Error')
    .andStatus(axiosError.response?.status ?? HttpStatusCode.BadRequest)
    .create();
}
