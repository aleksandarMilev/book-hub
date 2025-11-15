import type { GenreDetails, GenreName } from '@/features/genre/types/genre.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export const all = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.genres}`;
    const response = await http.get<GenreName[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.genre.all);
  }
};

export const details = async (id: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.genres}/${id}`;
    const response = await http.get<GenreDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.genre.byId);
  }
};
