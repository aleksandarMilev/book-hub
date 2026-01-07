import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { useFullInfo, useRemove } from '@/features/book/hooks/useCrud.js';
import { routes } from '@/shared/lib/constants/api.js';
import { slugify } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useDetailsPage = () => {
  const { id, slug } = useParams<{ id: string; slug?: string }>();

  const firstReviewRef = useRef<HTMLDivElement | null>(null);
  const [showFullDescription, setShowFullDescription] = useState(false);

  const navigate = useNavigate();
  const { userId, isAuthenticated, isAdmin } = useAuth();

  const { book, isFetching, error, refreshBook } = useFullInfo(id);
  const { showModal, toggleModal, deleteHandler } = useRemove(id, book?.title);

  const [isReviewCreated, setIsReviewCreated] = useState(false);
  const [isReviewEdited, setIsReviewEdited] = useState(false);

  useEffect(() => {
    if ((isReviewCreated || isReviewEdited) && firstReviewRef.current) {
      firstReviewRef.current.scrollIntoView({ behavior: 'smooth' });

      setIsReviewCreated(false);
      setIsReviewEdited(false);
    }
  }, [isReviewCreated, isReviewEdited, book?.reviews]);

  useEffect(() => {
    if (!book || !id) {
      return;
    }

    const canonicalSlug = slugify(book.title);
    if (!slug || slug !== canonicalSlug) {
      navigate(`${routes.book}/${id}/${canonicalSlug}`, { replace: true });
    }
  }, [book, id, slug, navigate]);

  return {
    showFullDescription,
    userId,
    isAuthenticated,
    isAdmin,
    error,
    isFetching,
    book,
    setShowFullDescription,
    toggleModal,
    id,
    setIsReviewEdited,
    refreshBook,
    firstReviewRef,
    showModal,
    deleteHandler,
    setIsReviewCreated,
  };
};
