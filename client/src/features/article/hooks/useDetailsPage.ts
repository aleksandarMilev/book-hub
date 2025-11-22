import { useParams } from 'react-router-dom';

import { useDetails, useRemove } from '@/features/article/hooks/useCrud.js';
import { formatIsoDate } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useDetailsPage = () => {
  const { isAdmin } = useAuth();
  const { id } = useParams<{ id: string }>();
  const { article, isFetching, error } = useDetails(id);
  const { showModal, toggleModal, deleteHandler } = useRemove(id, article?.title);

  const formattedDate = article
    ? formatIsoDate(article.createdOn, 'Publish date unavailable')
    : null;

  return {
    id,
    isAdmin,
    article,
    formattedDate,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
  };
};
