import { useCallback, useEffect, useMemo, useState } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

import * as api from '@/features/review/api/api.js';
import type { CreateReview, Review } from '@/features/review/types/review.js';
import { routes } from '@/shared/lib/constants/api.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import { IsCanceledError, IsError, toIntId } from '@/shared/lib/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useAll = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const { bookId } = useParams<{ bookId: string }>();
  const parsedBookId = useMemo(() => toIntId(bookId), [bookId]);

  const location = useLocation();
  const bookTitle = location.state as string | undefined;

  const [reviews, setReviews] = useState<Review[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(true);
  const [page, setPage] = useState<number>(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;
  const [totalItems, setTotalItems] = useState<number>(0);

  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));

  const fetchData = useCallback(
    async (pageNumber: number, signal?: AbortSignal) => {
      if (!token || !parsedBookId) {
        return;
      }

      try {
        setIsFetching(true);

        const data = await api.all(parsedBookId, pageNumber, pageSize, token, signal);

        setReviews(data?.items ?? []);
        setTotalItems(data?.totalItems ?? 0);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : errors.review.all;
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, parsedBookId, pageSize, navigate],
  );

  useEffect(() => {
    if (!parsedBookId) {
      navigate(routes.badRequest, { state: { message: 'Invalid book id.' } });
      return;
    }

    const controller = new AbortController();
    void fetchData(page, controller.signal);
    return () => controller.abort();
  }, [parsedBookId, page, fetchData, navigate]);

  const handleNextPage = useCallback(() => {
    if (page < totalPages) {
      setPage((p) => p + 1);
    }
  }, [page, totalPages]);

  const handlePreviousPage = useCallback(() => {
    if (page > 1) {
      setPage((p) => p - 1);
    }
  }, [page]);

  return {
    reviews,
    isFetching,
    bookTitle,
    page,
    totalPages,
    handleNextPage,
    handlePreviousPage,
    fetchData,
  };
};

export const useCreate = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (reviewData: CreateReview) => {
      if (!token) {
        return;
      }

      try {
        const created = await api.create(reviewData, token);
        return (created as { id?: number } | undefined)?.id;
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to create review.';
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );

  return createHandler;
};

export const useEdit = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (reviewId: number, reviewData: CreateReview) => {
      if (!token) {
        return;
      }

      try {
        return await api.edit(reviewId, reviewData, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to edit review.';
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );

  return editHandler;
};

export const useRemove = (id: number, refresh?: () => void | Promise<void>) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!showModal) {
      toggleModal();
      return;
    }

    if (!token) {
      return;
    }

    const controller = new AbortController();
    try {
      await api.remove(id, token, controller.signal);
      showMessage('Review was successfully deleted!', true);

      if (refresh) {
        await Promise.resolve(refresh());
      }
    } catch (error: unknown) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : 'Failed to delete review.';
      showMessage(message, false);

      navigate(routes.badRequest, { state: { message } });
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [showModal, token, id, refresh, showMessage, navigate, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};
