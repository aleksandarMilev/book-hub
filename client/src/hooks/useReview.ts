import {
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type Dispatch,
  type SetStateAction,
} from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

import * as api from '../api/review/reviewApi';
import type { Review, ReviewInput } from '../api/review/types/review';
import { routes } from '../common/constants/api';
import { pagination } from '../common/constants/defaultValues';
import { errors } from '../common/constants/messages';
import { parseId } from '../common/functions/utils';
import { useMessage } from '../contexts/message/messageContext';
import { UserContext } from '../contexts/user/userContext';


export function useReviewList() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const { bookId } = useParams<{ bookId: string }>();
  const parsedBookId = useMemo(() => parseId(bookId), [bookId]);

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
      if (!token || !parsedBookId) return;

      try {
        setIsFetching(true);

        const data = await api.all(parsedBookId, pageNumber, pageSize, token, signal);

        setReviews(data.items ?? []);
        setTotalItems(data.totalItems ?? 0);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') return;

        const message = error instanceof Error ? error.message : errors.review.list;
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
  }, [parsedBookId, page, token]);

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
}

export function useCreateReview() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const createHandler = useCallback(
    async (reviewData: ReviewInput) => {
      if (!token) {
        return;
      }

      try {
        const created = await api.create(reviewData, token);
        return created?.id as number;
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create review.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return createHandler;
}

export function useEditReview() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const editHandler = useCallback(
    async (reviewId: number, reviewData: ReviewInput) => {
      if (!token) {
        return;
      }

      try {
        return await api.edit(reviewId, reviewData, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit review.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

export function useVoteHandlers({
  id,
  hasProfile,
  upvoteCount,
  downvoteCount,
  setUpvoteCount,
  setDownvoteCount,
  setUpvoteClicked,
  setDownvoteClicked,
  onVote,
}: {
  id: number;
  hasProfile: boolean;
  upvoteCount: number;
  downvoteCount: number;
  setUpvoteCount: Dispatch<SetStateAction<number>>;
  setDownvoteCount: Dispatch<SetStateAction<number>>;
  setUpvoteClicked: Dispatch<SetStateAction<boolean>>;
  setDownvoteClicked: Dispatch<SetStateAction<boolean>>;
  onVote?: () => void | Promise<void>;
}) {
  const { token } = useContext(UserContext);

  const upvote = useUpvoteReview();
  const downvote = useDownvoteReview();

  const handleUpvote = useCallback(async () => {
    if (!token || !hasProfile) {
      return;
    }

    const success = await upvote(id, setUpvoteCount);
    if (success) {
      setDownvoteClicked(false);

      if (downvoteCount > 0) {
        setDownvoteCount((prev) => Math.max(0, prev - 1));
      }

      setUpvoteClicked(true);
      onVote?.();
    }
  }, [
    token,
    hasProfile,
    id,
    upvote,
    downvoteCount,
    setUpvoteCount,
    setDownvoteCount,
    setUpvoteClicked,
    setDownvoteClicked,
    onVote,
  ]);

  const handleDownvote = useCallback(async () => {
    if (!token || !hasProfile) {
      return;
    }

    const success = await downvote(id, setDownvoteCount);
    if (success) {
      setUpvoteClicked(false);

      if (upvoteCount > 0) {
        setUpvoteCount((prev) => Math.max(0, prev - 1));
      }

      setDownvoteClicked(true);
      onVote?.();
    }
  }, [
    token,
    hasProfile,
    id,
    downvote,
    upvoteCount,
    setUpvoteCount,
    setDownvoteCount,
    setUpvoteClicked,
    setDownvoteClicked,
    onVote,
  ]);

  return { handleUpvote, handleDownvote };
}

export const useRemoveReview = (id: number, refresh?: () => void | Promise<void>) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!showModal) {
      toggleModal();
      return;
    }

    const controller = new AbortController();
    try {
      const success = await api.remove(id, token, controller.signal);
      if (success) {
        showMessage('Review was successfully deleted!', true);
        if (refresh) {
          await Promise.resolve(refresh());
        }
      } else {
        showMessage(errors.review.delete, false);
      }
    } catch (error: unknown) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to delete review.';

      showMessage(message, false);
      navigate(routes.badRequest, { state: { message } });
    } finally {
      toggleModal();
    }

    return () => controller.abort();
  }, [showModal, id, token, refresh, showMessage, navigate, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

function useUpvoteReview() {
  const { token } = useContext(UserContext);

  const upvoteHandler = useCallback(
    async (id: number, setUpvoteCount?: Dispatch<SetStateAction<number>>) => {
      if (!token) {
        return;
      }

      const success = await api.upvote(id, token);
      if (success && setUpvoteCount) {
        setUpvoteCount((prev) => prev + 1);
      }

      return success;
    },
    [token],
  );

  return upvoteHandler;
}

function useDownvoteReview() {
  const { token } = useContext(UserContext);

  const downvoteHandler = useCallback(
    async (id: number, setDownvoteCount?: Dispatch<SetStateAction<number>>) => {
      if (!token) {
        return;
      }

      const success = await api.downvote(id, token);
      if (success && setDownvoteCount) {
        setDownvoteCount((prev) => prev + 1);
      }

      return success;
    },
    [token],
  );

  return downvoteHandler;
}
