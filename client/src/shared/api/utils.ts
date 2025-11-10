import axios, { AxiosError, type AxiosRequestConfig, HttpStatusCode } from 'axios';

import type { RouteKey } from '@/shared/api/types/routeKey';
import type { Routes } from '@/shared/api/types/routes';
import { baseAdminUrl, baseUrl } from '@/shared/lib/constants/api';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { HttpError } from '@/shared/types/errors/httpError';

export const httpClient = axios.create({ baseURL: baseUrl });
export const httpAdminClient = axios.create({ baseURL: baseAdminUrl });

export function getRoute<R extends Routes>(routes: R, key?: RouteKey<R>): string {
  const isString = typeof routes === 'string';
  if (isString) {
    return routes;
  }

  const record = routes as Record<string, string>;
  const hasValidKey = key && Object.prototype.hasOwnProperty.call(record, key);

  if (hasValidKey) {
    return record[key]!;
  }

  const first = Object.values(record)[0];
  if (first) {
    return first;
  }

  throw new Error(`Unknown route key: ${String(key)}`);
}

export const getAuthConfig = (token: string, signal?: AbortSignal) => {
  const config: AxiosRequestConfig = {
    headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' },
  };

  if (signal) {
    config.signal = signal;
  }

  return config;
};

export const handleRequestError = (error: unknown, message: string): never => {
  const isRequestCanceled = axios.isCancel?.(error) || (IsError(error) && IsCanceledError(error));
  if (isRequestCanceled) {
    throw error;
  }

  const axiosError = error as AxiosError;
  throw HttpError.with()
    .message(message)
    .name('Error')
    .status(axiosError.response?.status ?? HttpStatusCode.BadRequest)
    .create();
};
