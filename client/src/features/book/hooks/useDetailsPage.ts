import { useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router-dom';

import { useFullInfo, useRemove } from '@/features/book/hooks/useCrud.js';
import { toIntId } from '@/shared/lib/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const firstReviewRef = useRef<HTMLDivElement | null>(null);
  const [showFullDescription, setShowFullDescription] = useState(false);

  const { userId, hasProfile, isAdmin } = useAuth();

  const { book, isFetching, error, refreshBook } = useFullInfo(parsedId, disable);
  const { showModal, toggleModal, deleteHandler } = useRemove(parsedId, disable, book?.title);

  const [isReviewCreated, setIsReviewCreated] = useState(false);
  const [isReviewEdited, setIsReviewEdited] = useState(false);

  useEffect(() => {
    if ((isReviewCreated || isReviewEdited) && firstReviewRef.current) {
      firstReviewRef.current.scrollIntoView({ behavior: 'smooth' });

      setIsReviewCreated(false);
      setIsReviewEdited(false);
    }
  }, [isReviewCreated, isReviewEdited, book?.reviews]);

  return {
    showFullDescription,
    userId,
    hasProfile,
    isAdmin,
    error,
    isFetching,
    book,
    setShowFullDescription,
    toggleModal,
    parsedId,
    setIsReviewEdited,
    refreshBook,
    firstReviewRef,
    showModal,
    deleteHandler,
    setIsReviewCreated,
  };
};
