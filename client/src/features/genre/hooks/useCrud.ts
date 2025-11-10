import api from '@/features/genre/api/api';
import type { GenreDetails, GenreName } from '@/features/genre/types/genre';
import baseCrudBuilder from '@/shared/hooks/useCrud/baseCrudBuilder';
import { errors } from '@/shared/lib/constants/errorMessages';

export const { useDetails } = baseCrudBuilder<GenreName, GenreDetails, null>()
  .with()
  .api(api)
  .and()
  .resource('Genre')
  .and()
  .errors(errors.genre)
  .create();
