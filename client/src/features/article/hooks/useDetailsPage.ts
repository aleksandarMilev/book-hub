import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { useDetails, useRemove } from '@/features/article/hooks/useCrud';
import { routes } from '@/shared/lib/constants/api';
import { formatIsoDate, slugify } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';

export const useDetailsPage = () => {
  const { isAdmin } = useAuth();
  const navigate = useNavigate();
  const { id, slug } = useParams<{ id: string; slug: string }>();
  const { article, readingMinutes, isFetching, error } = useDetails(id);
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
    readingMinutes,
    isUpdated,
    formattedDate,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
  };
};


