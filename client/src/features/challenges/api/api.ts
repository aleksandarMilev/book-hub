import type {
  ReadingChallenge,
  ReadingChallengeProgress,
  ReadingStreak,
  UpsertReadingChallengePayload,
} from '@/features/challenges/types/challenge';
import { getAuthConfig, http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export const get = async (year: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.readingChallenges.base}/${year}`;
    const { data } = await http.get<ReadingChallenge | null>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.challenges.get);
  }
};

export const upsert = async (
  payload: UpsertReadingChallengePayload,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = routes.readingChallenges.base;
    await http.put(url, payload, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.challenges.upsert);
  }
};

export const progress = async (year: number, token: string, signal?: AbortSignal) => {
  try {
    const url = routes.readingChallenges.progress(year);
    const { data } = await http.get<ReadingChallengeProgress | null>(
      url,
      getAuthConfig(token, signal),
    );

    return data;
  } catch (error) {
    processError(error, errors.challenges.progress);
  }
};

export const streak = async (token: string, signal?: AbortSignal) => {
  try {
    const url = routes.readingChallenges.streak;
    const { data } = await http.get<ReadingStreak>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.challenges.streak);
  }
};

export const checkInToday = async (token: string, signal?: AbortSignal) => {
  try {
    const url = routes.readingChallenges.checkIn;
    await http.post(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.challenges.checkIn);
  }
};
