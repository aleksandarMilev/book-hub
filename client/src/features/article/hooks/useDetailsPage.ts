import { useParams } from 'react-router-dom';

import * as hooks from '@/features/article/hooks/useCrud';
import { formatIsoDate, toIntId } from '@/shared/lib/utils';
import { useAuth } from '@/shared/stores/auth/auth';

export const useDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { isAdmin } = useAuth();
  const { article, isFetching, error } = hooks.useDetails(parsedId, disable);
  const { showModal, toggleModal, deleteHandler } = hooks.useRemove(
    parsedId,
    disable,
    article?.title,
  );

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
