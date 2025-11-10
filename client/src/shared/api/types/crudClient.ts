import type { AxiosRequestConfig, AxiosResponse } from 'axios';

export type CrudClients<_TAll = unknown, _TDetails = unknown, _TCreate = unknown> = {
  all?: <TResponse = unknown>(
    url: string,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<TResponse>>;
  byId?: <TResponse = unknown>(
    url: string,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<TResponse>>;
  post?: <TResponse = unknown>(
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<TResponse>>;
  put?: <TResponse = unknown>(
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<TResponse>>;
  delete?: <TResponse = unknown>(
    url: string,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<TResponse>>;
  publicPost?: <TResponse = unknown>(
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<TResponse>>;
};
