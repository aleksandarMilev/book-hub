import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { useDetails, useRemove } from '@/features/article/hooks/useCrud.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate, slugify } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useDetailsPage = () => {
  const { isAdmin } = useAuth();
  const navigate = useNavigate();
  const { id, slug } = useParams<{ id: string; slug: string }>();
  const { article, isFetching, error } = useDetails(id);
  const { showModal, toggleModal, deleteHandler } = useRemove(id, article?.title);

  useEffect(() => {
    if (!article || !id) {
      return;
    }

    const canonicalSlug = slugify(article.title);

    if (!slug || slug !== canonicalSlug) {
      navigate(`${routes.articles}/${id}/${canonicalSlug}`, { replace: true });
    }
  }, [article, id, slug, navigate]);

  const formattedDate = article
    ? formatIsoDate(article.modifiedOn ?? article.createdOn, 'Publish date unavailable')
    : null;

  const isUpdated = article?.modifiedOn != null;

  return {
    id,
    isAdmin,
    article,
    isUpdated,
    formattedDate,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
  };
};
