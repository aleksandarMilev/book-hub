import { useCallback, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/review/reviewApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { CreateReview } from '../api/review/types/createReview';

export function useCreateReview() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (reviewData: CreateReview): Promise<number | void> => {
      if (!token) return;

      try {
        const createdReview = await api.create(reviewData, token);
        return createdReview.id;
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
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (reviewId: number, reviewData: CreateReview): Promise<boolean | void> => {
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

export function useUpvoteReview() {
  const { token } = useContext(UserContext);

  const upvoteHandler = useCallback(
    async (
      id: number,
      setUpvoteCount: React.Dispatch<React.SetStateAction<number>>,
    ): Promise<void> => {
      if (!token) {
        return;
      }

      const success = await api.upvote(id, token);
      if (success) {
        setUpvoteCount((prev) => prev + 1);
      }
    },
    [token],
  );

  return upvoteHandler;
}

export function useDownvoteReview() {
  const { token } = useContext(UserContext);

  const downvoteHandler = useCallback(
    async (
      id: number,
      setDownvoteCount: React.Dispatch<React.SetStateAction<number>>,
    ): Promise<void> => {
      if (!token) {
        return;
      }

      const success = await api.downvote(id, token);
      if (success) {
        setDownvoteCount((prev) => prev + 1);
      }
    },
    [token],
  );

  return downvoteHandler;
}
