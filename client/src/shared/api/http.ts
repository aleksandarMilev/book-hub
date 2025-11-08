import axios, { AxiosError, type AxiosRequestConfig, HttpStatusCode } from 'axios';

import type { CrudClients } from '@/shared/api/types/crudClient';
import { baseAdminUrl, baseUrl } from '@/shared/lib/constants/api';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { HttpError } from '@/shared/types/errors/httpError';

export const httpClient = axios.create({ baseURL: baseUrl });
export const httpAdminClient = axios.create({ baseURL: baseAdminUrl });

export function createBaseApi<TCreate, TDetails>(
  clients: CrudClients<TDetails, TCreate>,
  baseRoute: string,
  errorMessages: {
    get: string;
    create: string;
    edit: string;
    delete: string;
  },
) {
  return {
    async details(id: number, token: string, signal?: AbortSignal) {
      try {
        const response = await clients.get<TDetails>(
          `${baseRoute}/${id}`,
          getAuthConfig(token, signal),
        );

        return response.data;
      } catch (error) {
        return handleRequestError(error, errorMessages.get); // return to make ts happy
      }
    },

    async create(data: TCreate, token: string, signal?: AbortSignal) {
      try {
        const response = await clients.post<{ id: number }>(
          baseRoute,
          data,
          getAuthConfig(token, signal),
        );

        return response.data.id;
      } catch (error) {
        return handleRequestError(error, errorMessages.create);
      }
    },

    async edit(id: number, data: TCreate, token: string, signal?: AbortSignal) {
      try {
        await clients.put(`${baseRoute}/${id}`, data, getAuthConfig(token, signal));
        return true;
      } catch (error) {
        return handleRequestError(error, errorMessages.edit);
      }
    },

    async remove(id: number, token: string, signal?: AbortSignal) {
      try {
        await clients.delete(`${baseRoute}/${id}`, getAuthConfig(token, signal));
        return true;
      } catch (error) {
        return handleRequestError(error, errorMessages.delete);
      }
    },
  };
}

const getAuthConfig = (token: string, signal?: AbortSignal) => {
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

const handleRequestError = (error: unknown, message: string): never => {
  const isRequestCanceled = axios.isCancel?.(error) || (IsError(error) && IsCanceledError(error));
  if (isRequestCanceled) {
    throw error;
  }

  const axiosError = error as AxiosError;
  throw HttpError.with()
    .message(message)
    .andName('Error')
    .andStatus(axiosError.response?.status ?? HttpStatusCode.BadRequest)
    .create();
};
