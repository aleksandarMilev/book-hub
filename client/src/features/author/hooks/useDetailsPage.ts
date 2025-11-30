import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { useDetails, useRemove } from '@/features/author/hooks/useCrud.js';
import { routes } from '@/shared/lib/constants/api.js';
import { slugify } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useDetailsPage = () => {
  const navigate = useNavigate();
  const { id, slug } = useParams<{ id: string; slug: string }>();

  const { showMessage } = useMessage();
  const { token, isAdmin, userId } = useAuth();
  const { author, isFetching, error } = useDetails(id);
  const { showModal, toggleModal, deleteHandler } = useRemove(id, author?.name);

  useEffect(() => {
    if (!author || !id) {
      return;
    }

    const canonicalSlug = slugify(author.name);

    if (!slug || slug !== canonicalSlug) {
      navigate(`${routes.author}/${id}/${canonicalSlug}`, { replace: true });
    }
  }, [author, id, slug, navigate]);

  return {
    id,
    token,
    isAdmin,
    userId,
    author,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
    navigate,
    showMessage,
  };
};
