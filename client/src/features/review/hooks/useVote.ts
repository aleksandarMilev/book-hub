import { type Dispatch, type SetStateAction, useCallback } from 'react';

import * as api from '@/features/review/api/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export const useUpvote = () => {
  const { token } = useAuth();

  const upvoteHandler = useCallback(
    async (id: string, setUpvoteCount?: Dispatch<SetStateAction<number>>) => {
      if (!token) return false;

      try {
        const success = await api.upvote(id, token);
        if (success && setUpvoteCount) {
          setUpvoteCount((prev) => prev + 1);
        }

        return !!success;
      } catch {
        return false;
      }
    },
    [token],
  );

  return upvoteHandler;
};

export const useDownvote = () => {
  const { token } = useAuth();

  const downvoteHandler = useCallback(
    async (id: string, setDownvoteCount?: Dispatch<SetStateAction<number>>) => {
      if (!token) {
        return false;
      }

      try {
        const success = await api.downvote(id, token);
        if (success && setDownvoteCount) {
          setDownvoteCount((prev) => prev + 1);
        }

        return !!success;
      } catch {
        return false;
      }
    },
    [token],
  );

  return downvoteHandler;
};

export const useVoteHandlers = ({
  id,
  isAuthenticated,
  upvoteCount,
  downvoteCount,
  setUpvoteCount,
  setDownvoteCount,
  setUpvoteClicked,
  setDownvoteClicked,
}: {
  id: string;
  isAuthenticated: boolean;
  upvoteCount: number;
  downvoteCount: number;
  setUpvoteCount: Dispatch<SetStateAction<number>>;
  setDownvoteCount: Dispatch<SetStateAction<number>>;
  setUpvoteClicked: Dispatch<SetStateAction<boolean>>;
  setDownvoteClicked: Dispatch<SetStateAction<boolean>>;
}) => {
  const { token } = useAuth();

  const upvote = useUpvote();
  const downvote = useDownvote();

  const handleUpvote = useCallback(async () => {
    if (!token || !isAuthenticated) return;

    const success = await upvote(id, setUpvoteCount);
    if (success) {
      setDownvoteClicked(false);
      if (downvoteCount > 0) {
        setDownvoteCount((prev) => Math.max(0, prev - 1));
      }
      setUpvoteClicked(true);
    }
  }, [
    token,
    isAuthenticated,
    id,
    upvote,
    downvoteCount,
    setUpvoteCount,
    setDownvoteCount,
    setUpvoteClicked,
    setDownvoteClicked,
  ]);

  const handleDownvote = useCallback(async () => {
    if (!token || !isAuthenticated) return;

    const success = await downvote(id, setDownvoteCount);
    if (success) {
      setUpvoteClicked(false);
      if (upvoteCount > 0) {
        setUpvoteCount((prev) => Math.max(0, prev - 1));
      }
      setDownvoteClicked(true);
    }
  }, [
    token,
    isAuthenticated,
    id,
    downvote,
    upvoteCount,
    setUpvoteCount,
    setDownvoteCount,
    setUpvoteClicked,
    setDownvoteClicked,
  ]);

  return { handleUpvote, handleDownvote };
};
