import type { GenreDetails, GenreName } from '@/features/genre/types/genre.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export const all = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.genres}`;
    const { data } = await http.get<GenreName[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.genre.all);
  }
};

export const details = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.genres}/${id}`;
    const { data } = await http.get<GenreDetails>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.genre.byId);
  }
};
