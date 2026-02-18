import { useCallback, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';

import {
  ReadingGoalType,
  type ReadingChallengeProgress,
  type ReadingStreak,
} from '@/features/challenges/types/challenge';
import {
  useCheckInToday,
  useGetProgress,
  useGetStreak,
  useUpsertChallenge,
} from '@/features/challenges/hooks/useCrud';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils';
import { useMessage } from '@/shared/stores/message/message';

export const useChallengePage = () => {
  const { showMessage } = useMessage();
  const { t } = useTranslation('challenges');

  const getProgress = useGetProgress();
  const getStreak = useGetStreak();
  const upsert = useUpsertChallenge();
  const checkIn = useCheckInToday();

  const year = useMemo(() => new Date().getFullYear(), []);
  const [goalType, setGoalType] = useState<ReadingGoalType>(ReadingGoalType.Books);
  const [goalValue, setGoalValue] = useState<number>(24);

  const [progress, setProgress] = useState<ReadingChallengeProgress | null>(null);
  const [streak, setStreak] = useState<ReadingStreak | null>(null);

  const [isFetching, setIsFetching] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [isCheckingIn, setIsCheckingIn] = useState(false);

  const [error, setError] = useState<Error | null>(null);

  const refresh = useCallback(async () => {
    const controller = new AbortController();

    try {
      setIsFetching(true);

      const [progress, streak] = await Promise.all([
        getProgress(year, controller.signal),
        getStreak(controller.signal),
      ]);

      setProgress(progress ?? null);
      setStreak(streak ?? null);

      if (progress) {
        setGoalType(progress.goalType);
        setGoalValue(progress.goalValue);
      }
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      setError(error as Error);
    } finally {
      setIsFetching(false);
      controller.abort();
    }
  }, [getProgress, getStreak, year]);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const saveGoal = useCallback(async () => {
    const controller = new AbortController();

    try {
      setIsSaving(true);

      await upsert(
        {
          year,
          goalType,
          goalValue: Number(goalValue),
        },
        controller.signal,
      );

      showMessage(t('messages.saveSuccess'), true);
      await refresh();
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const fallback = t('messages.saveError');
      const message = IsError(error) ? error.message : fallback;
      showMessage(message, false);
    } finally {
      setIsSaving(false);
      controller.abort();
    }
  }, [goalType, goalValue, refresh, showMessage, t, upsert, year]);

  const checkInTodayHandler = useCallback(async () => {
    const controller = new AbortController();

    try {
      setIsCheckingIn(true);

      await checkIn(controller.signal);
      showMessage(t('messages.checkInSuccess'), true);

      await refresh();
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const fallback = t('messages.checkInError');
      const message = IsError(error) ? error.message : fallback;
      showMessage(message, false);
    } finally {
      setIsCheckingIn(false);
      controller.abort();
    }
  }, [checkIn, refresh, showMessage, t]);

  const unitLabel = goalType === ReadingGoalType.Pages ? 'pages' : 'books';

  return {
    year,
    goalType,
    setGoalType,
    goalValue,
    setGoalValue,
    progress,
    streak,
    isFetching,
    isSaving,
    isCheckingIn,
    error,
    unitLabel,
    saveGoal,
    checkInTodayHandler,
    refresh,
  };
};
