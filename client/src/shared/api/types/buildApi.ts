import type { RouteKey } from '@/shared/api/types/routeKey';
import type { Routes } from '@/shared/api/types/routes';

export type BuiltApi<TAll, TDetails, TCreate, R extends Routes> = {
  all: (token: string, signal?: AbortSignal, routeKey?: RouteKey<R>) => Promise<TAll>;
  details: (
    id: number,
    token: string,
    signal?: AbortSignal,
    routeKey?: RouteKey<R>,
  ) => Promise<TDetails>;
  create: (
    data: TCreate,
    token: string,
    signal?: AbortSignal,
    routeKey?: RouteKey<R>,
  ) => Promise<number>;
  edit: (
    id: number,
    data: TCreate,
    token: string,
    signal?: AbortSignal,
    routeKey?: RouteKey<R>,
  ) => Promise<true>;
  remove: (
    id: number,
    token: string,
    signal?: AbortSignal,
    routeKey?: RouteKey<R>,
  ) => Promise<true>;
  patch: (
    id: number,
    data: TCreate,
    token: string,
    signal?: AbortSignal,
    routeKey?: RouteKey<R>,
  ) => Promise<true>;
  publicPost: <TRequest, TResponse>(
    data: TRequest,
    signal?: AbortSignal,
    routeKey?: RouteKey<R>,
  ) => Promise<TResponse>;
};
