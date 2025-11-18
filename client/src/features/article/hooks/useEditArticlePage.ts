import { useParams } from 'react-router-dom';

import * as hooks from '@/features/article/hooks/useCrud.js';
export const useEditArticlePage = () => {
  const { id } = useParams<{ id: string }>();
  const { article, isFetching, error } = hooks.useDetails(id);

  return {
    article,
    isFetching,
    error,
  };
};
