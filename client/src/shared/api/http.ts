import axios, { type AxiosRequestConfig } from 'axios';

import { baseAdminUrl, baseUrl } from '@/shared/lib/constants/api';
import { IsCanceledError } from '@/shared/lib/utils/utils';

export const http = axios.create({ baseURL: baseUrl });
export const httpAdmin = axios.create({ baseURL: baseAdminUrl });

export const getAuthConfig = (token: string, signal?: AbortSignal): AxiosRequestConfig => {
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
};

export const getPublicConfig = (signal?: AbortSignal): AxiosRequestConfig => {
  const config: AxiosRequestConfig = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

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
