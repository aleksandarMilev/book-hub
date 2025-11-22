import { useParams } from 'react-router-dom';

import { useDetails } from '@/features/author/hooks/useCrud.js';
import { toIntId } from '@/shared/lib/utils/utils.js';

export const useEditPage = () => {
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { author, isFetching, error } = useDetails(parsedId, disable);

  return {
    author,
    isFetching,
    error,
  };
};
