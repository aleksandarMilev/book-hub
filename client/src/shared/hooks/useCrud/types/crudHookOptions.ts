import type { CrudApi } from '@/shared/hooks/useCrud/types/crudApi';
import type { CrudErrors } from '@/shared/hooks/useCrud/types/crudErrors';

export interface CrudHookOptions<TAll, TDetails, TCreate> {
  api: CrudApi<TAll, TDetails, TCreate>;
  resourceName: string;
  errors: CrudErrors;
}
