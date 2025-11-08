import api from '@/features/genre/api/api';
import { createCrudHooks } from '@/shared/hooks/useCrud/useCrud';
import { errors } from '@/shared/lib/constants/errorMessages';

export const { useDetails } = createCrudHooks({
  api,
  resourceName: 'Genre',
  errors: errors.genre,
});
