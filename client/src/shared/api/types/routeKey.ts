import type { Routes } from '@/shared/api/types/routes';

export type RouteKey<R extends Routes> = R extends string ? never : Extract<keyof R, string>;
