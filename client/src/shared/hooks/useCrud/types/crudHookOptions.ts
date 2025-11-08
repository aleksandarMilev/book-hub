import type { CrudApi } from '@/shared/hooks/useCrud/types/crudApi';

export interface CrudHookOptions<TCreate, TDetails> {
  api: CrudApi<TCreate, TDetails>;
  resourceName: string;
  errors: {
    get: string;
    create: string;
    edit: string;
    delete: string;
    notFound?: string;
  };
}
