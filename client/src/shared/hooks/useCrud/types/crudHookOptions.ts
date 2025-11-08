import type { CrudApi } from '@/shared/hooks/useCrud/types/crudApi';

export interface CrudHookOptions<TAll, TDetails, TCreate> {
  api: CrudApi<TAll, TDetails, TCreate>;
  resourceName: string;
  errors: {
    all: string;
    byId: string;
    create: string;
    edit: string;
    delete: string;
  };
}
