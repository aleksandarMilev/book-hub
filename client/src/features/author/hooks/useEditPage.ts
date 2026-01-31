import { useParams } from 'react-router-dom';

import { useDetails } from '@/features/author/hooks/useCrud';

export const useEditPage = () => {
  const { id } = useParams<{ id: string }>();
  const { author, isFetching, error } = useDetails(id);

  return {
    author,
    isFetching,
    error,
  };
};


