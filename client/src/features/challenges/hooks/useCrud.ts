import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/challenges/api/api';
import type { UpsertReadingChallengePayload } from '@/features/challenges/types/challenge';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import { IsError } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';

export const useGetChallenge = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (year: number, signal?: AbortSignal) => {
      try {
        return await api.get(year, token, signal);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.challenges.get;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useUpsertChallenge = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (payload: UpsertReadingChallengePayload, signal?: AbortSignal) => {
      try {
        return await api.upsert(payload, token, signal);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.challenges.upsert;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useGetProgress = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (year: number, signal?: AbortSignal) => {
      try {
        return await api.progress(year, token, signal);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.challenges.progress;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useGetStreak = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (signal?: AbortSignal) => {
      try {
        return await api.streak(token, signal);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.challenges.streak;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useCheckInToday = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (signal?: AbortSignal) => {
      try {
        return await api.checkInToday(token, signal);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.challenges.checkIn;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};
