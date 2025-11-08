export interface CrudApi<TCreate, TDetails> {
  details: (id: number, token: string, signal?: AbortSignal) => Promise<TDetails>;
  create: (data: TCreate, token: string, signal?: AbortSignal) => Promise<number>;
  edit: (id: number, data: TCreate, token: string, signal?: AbortSignal) => Promise<boolean>;
  remove: (id: number, token: string, signal?: AbortSignal) => Promise<boolean>;
}
