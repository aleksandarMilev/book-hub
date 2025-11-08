import axios, {
  AxiosError,
  type AxiosRequestConfig,
  type AxiosResponse,
  HttpStatusCode,
} from 'axios';

import type { CrudClients } from '@/shared/api/types/crudClient';
import { baseAdminUrl, baseUrl } from '@/shared/lib/constants/api';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { HttpError } from '@/shared/types/errors/httpError';

export const httpClient = axios.create({ baseURL: baseUrl });
export const httpAdminClient = axios.create({ baseURL: baseAdminUrl });

export function createBaseApi<TAll = null, TDetails = null, TCreate = null>(
  clients: CrudClients<TAll, TDetails, TCreate> & {
    publicPost?: <TResponse = unknown>(
      url: string,
      data?: unknown,
      config?: AxiosRequestConfig,
    ) => Promise<AxiosResponse<TResponse>>;
  },
  baseRoute: string | { [key: string]: string },
  errorMessages: {
    all: string;
    byId: string;
    create: string;
    edit: string;
    delete: string;
  },
) {
  return {
    async publicPost<TRequest, TResponse>(
      data: TRequest,
      signal?: AbortSignal,
      routeKey?: string,
    ): Promise<TResponse> {
      if (!clients.publicPost) {
        throw new Error('publicPost() is not implemented!');
      }

      try {
        const config: AxiosRequestConfig = {
          headers: { 'Content-Type': 'application/json' },
          ...(signal ? { signal } : {}),
        };

        const route = getRoute(baseRoute, routeKey);
        const response = await clients.publicPost<TResponse>(route, data, config);

        return response.data;
      } catch (error) {
        return handleRequestError(error, errorMessages.create);
      }
    },

    async all(token: string, signal?: AbortSignal) {
      if (!clients.all) throw new Error('all() is not implemented!');
      try {
        const response = await clients.all<TAll>(getRoute(baseRoute), getAuthConfig(token, signal));
        return response.data;
      } catch (error) {
        return handleRequestError(error, errorMessages.all);
      }
    },

    async details(id: number, token: string, signal?: AbortSignal) {
      if (!clients.byId) throw new Error('byId() is not implemented!');
      try {
        const response = await clients.byId<TDetails>(
          `${getRoute(baseRoute)}/${id}`,
          getAuthConfig(token, signal),
        );
        return response.data;
      } catch (error) {
        return handleRequestError(error, errorMessages.byId);
      }
    },

    async create(data: TCreate, token: string, signal?: AbortSignal, routeKey?: string) {
      if (!clients.post) throw new Error('create() is not implemented!');
      try {
        const response = await clients.post<{ id: number }>(
          getRoute(baseRoute, routeKey),
          data,
          getAuthConfig(token, signal),
        );
        return response.data.id;
      } catch (error) {
        return handleRequestError(error, errorMessages.create);
      }
    },

    async edit(id: number, data: TCreate, token: string, signal?: AbortSignal) {
      if (!clients.put) throw new Error('edit() is not implemented!');
      try {
        await clients.put(`${getRoute(baseRoute)}/${id}`, data, getAuthConfig(token, signal));
        return true;
      } catch (error) {
        return handleRequestError(error, errorMessages.edit);
      }
    },

    async remove(id: number, token: string, signal?: AbortSignal) {
      if (!clients.delete) throw new Error('remove() is not implemented!');
      try {
        await clients.delete(`${getRoute(baseRoute)}/${id}`, getAuthConfig(token, signal));
        return true;
      } catch (error) {
        return handleRequestError(error, errorMessages.delete);
      }
    },
  };
}

const getRoute = (baseRoute: string | { [key: string]: string }, key?: string) => {
  if (typeof baseRoute === 'string') {
    return baseRoute;
  }

  if (key && baseRoute[key]) {
    return baseRoute[key];
  }

  const first = Object.values(baseRoute)[0];
  if (first) {
    return first;
  }

  throw new Error(`Unknown route key: ${key}`);
};

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
