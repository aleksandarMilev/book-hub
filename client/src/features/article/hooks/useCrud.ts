import api from '@/features/article/api/api';
import { createCrudHooks } from '@/shared/hooks/useCrud/useCrud';
import { errors } from '@/shared/lib/constants/errorMessages';

export const { useDetails, useCreate, useEdit, useRemove } = createCrudHooks({
  api,
  resourceName: 'Article',
  errors: errors.article,
});
