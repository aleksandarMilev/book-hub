import { useParams } from 'react-router-dom';

import * as hooks from '@/features/article/hooks/useCrud';
import { toIntId } from '@/shared/lib/utils';

export function useEditArticlePage() {
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { data: article, isFetching, error } = hooks.useDetails(parsedId, disable);

  return {
    article,
    isFetching,
    error,
  };
}
