import { useParams } from 'react-router-dom';

import { useDetails } from '@/features/article/hooks/useCrud.js';

export const useEditArticlePage = () => {
  const { id } = useParams<{ id: string }>();
  const { article, isFetching, error } = useDetails(id);

  return {
    article,
    isFetching,
    error,
  };
};
