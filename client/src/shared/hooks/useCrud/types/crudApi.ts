export interface CrudApi<TAll, TDetails, TCreate> {
  all?: (token: string, signal?: AbortSignal) => Promise<TAll>;
  details?: (id: number, token: string, signal?: AbortSignal) => Promise<TDetails>;
  create?: (data: TCreate, token: string, signal?: AbortSignal) => Promise<number>;
  edit?: (id: number, data: TCreate, token: string, signal?: AbortSignal) => Promise<boolean>;
  remove?: (id: number, token: string, signal?: AbortSignal) => Promise<boolean>;
  patch?: (id: number, data: TCreate, token: string, signal?: AbortSignal) => Promise<boolean>;
}
