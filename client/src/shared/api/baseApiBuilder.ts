import { type AxiosInstance, type AxiosRequestConfig } from 'axios';

import type { BuiltApi } from '@/shared/api/types/buildApi';
import type { CrudClients } from '@/shared/api/types/crudClient';
import type { ErrorMessages } from '@/shared/api/types/errorMessages';
import type { RouteKey } from '@/shared/api/types/routeKey';
import type { Routes } from '@/shared/api/types/routes';
import {
  getAuthConfig,
  getRoute,
  handleRequestError,
  httpAdminClient,
  httpClient,
} from '@/shared/api/utils';

export default <TAll = null, TDetails = null, TCreate = null>() =>
  new BaseApiBuilder<TAll, TDetails, TCreate>();

class BaseApiBuilder<TAll = null, TDetails = null, TCreate = null, R extends Routes = string> {
  private _clients: CrudClients<TAll, TDetails, TCreate> = {};
  private _routes!: R;
  private _errors!: ErrorMessages;

  with() {
    return this;
  }

  and() {
    return this;
  }

  getClient(client?: AxiosInstance['get']) {
    this._clients.all = client ?? httpClient.get;
    this._clients.byId = client ?? httpClient.get;

    return this;
  }

  postClient(client?: AxiosInstance['post']) {
    this._clients.post = client ?? httpAdminClient.post;

    return this;
  }

  putClient(client?: AxiosInstance['put']) {
    this._clients.put = client ?? httpAdminClient.put;

    return this;
  }

  deleteClient(client?: AxiosInstance['delete']) {
    this._clients.delete = client ?? httpAdminClient.delete;

    return this;
  }

  patchClient(client?: AxiosInstance['patch']) {
    this._clients.patch = client ?? httpAdminClient.patch;

    return this;
  }

  publicPostClient(client?: AxiosInstance['post']) {
    this._clients.publicPost = client ?? httpClient.post;

    return this;
  }

  withClients(clients: Partial<CrudClients<TAll, TDetails, TCreate>>) {
    this._clients = { ...this._clients, ...clients };

    return this;
  }

  routes<NR extends Routes>(routes: NR) {
    (this as unknown as BaseApiBuilder<TAll, TDetails, TCreate, NR>)._routes = routes;

    return this as unknown as BaseApiBuilder<TAll, TDetails, TCreate, NR>;
  }

  errors(errors: ErrorMessages) {
    this._errors = errors;

    return this;
  }

  create(): BuiltApi<TAll, TDetails, TCreate, R> {
    const clients = this._clients;
    const routes = this._routes;
    const errors = this._errors;

    if (!routes) {
      throw new Error('Routes are required. Use .withRoutes(...)');
    }

    if (!errors) {
      throw new Error('Error messages are required. Use .withErrors(...)');
    }

    return {
      async publicPost<TRequest, TResponse>(
        data: TRequest,
        signal?: AbortSignal,
        routeKey?: RouteKey<R>,
      ) {
        if (!clients.publicPost) {
          throw new Error('publicPost() is not implemented!');
        }

        try {
          const config: AxiosRequestConfig = {
            headers: { 'Content-Type': 'application/json' },
            ...(signal ? { signal } : {}),
          };

          const route = getRoute(routes, routeKey);
          const response = await clients.publicPost<TResponse>(route, data, config);

          return response.data as TResponse;
        } catch (error) {
          return handleRequestError(error, errors.create);
        }
      },

      async all(token: string, signal?: AbortSignal, routeKey?: RouteKey<R>) {
        if (!clients.all) {
          throw new Error('all() is not implemented!');
        }

        try {
          const response = await clients.all<TAll>(
            getRoute(routes, routeKey),
            getAuthConfig(token, signal),
          );

          return response.data as TAll;
        } catch (error) {
          return handleRequestError(error, errors.all);
        }
      },

      async details(id: number, token: string, signal?: AbortSignal, routeKey?: RouteKey<R>) {
        if (!clients.byId) {
          throw new Error('byId() is not implemented!');
        }

        try {
          const response = await clients.byId<TDetails>(
            `${getRoute(routes, routeKey)}/${id}`,
            getAuthConfig(token, signal),
          );

          return response.data as TDetails;
        } catch (error) {
          return handleRequestError(error, errors.byId);
        }
      },

      async create(data: TCreate, token: string, signal?: AbortSignal, routeKey?: RouteKey<R>) {
        if (!clients.post) {
          throw new Error('create() is not implemented!');
        }

        try {
          const response = await clients.post<{ id: number }>(
            getRoute(routes, routeKey),
            data,
            getAuthConfig(token, signal),
          );

          return response.data.id;
        } catch (error) {
          return handleRequestError(error, errors.create);
        }
      },

      async edit(
        id: number,
        data: TCreate,
        token: string,
        signal?: AbortSignal,
        routeKey?: RouteKey<R>,
      ) {
        if (!clients.put) {
          throw new Error('edit() is not implemented!');
        }

        try {
          await clients.put(
            `${getRoute(routes, routeKey)}/${id}`,
            data,
            getAuthConfig(token, signal),
          );

          return true as const;
        } catch (error) {
          return handleRequestError(error, errors.edit);
        }
      },

      async remove(id: number, token: string, signal?: AbortSignal, routeKey?: RouteKey<R>) {
        if (!clients.delete) {
          throw new Error('remove() is not implemented!');
        }

        try {
          await clients.delete(`${getRoute(routes, routeKey)}/${id}`, getAuthConfig(token, signal));
          return true as const;
        } catch (error) {
          return handleRequestError(error, errors.delete);
        }
      },

      async patch(
        id: number,
        data: TCreate,
        token: string,
        signal?: AbortSignal,
        routeKey?: RouteKey<R>,
      ) {
        if (!clients.patch) {
          throw new Error('patch() is not implemented!');
        }

        try {
          await clients.patch(
            `${getRoute(routes, routeKey)}/${id}`,
            data,
            getAuthConfig(token, signal),
          );

          return true as const;
        } catch (error) {
          return handleRequestError(error, errors.edit);
        }
      },
    };
  }
}
