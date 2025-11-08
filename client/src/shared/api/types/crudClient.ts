import type { AxiosRequestConfig, AxiosResponse } from 'axios';

export interface CrudClients<TDetails, TCreate> {
  get: <T = TDetails>(url: string, config?: AxiosRequestConfig) => Promise<AxiosResponse<T>>;
  post: <T = { id: number }>(
    url: string,
    data?: TCreate,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<T>>;
  put: <T = void>(
    url: string,
    data?: TCreate,
    config?: AxiosRequestConfig,
  ) => Promise<AxiosResponse<T>>;
  delete: <T = void>(url: string, config?: AxiosRequestConfig) => Promise<AxiosResponse<T>>;
}
