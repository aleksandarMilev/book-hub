import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { http } from '../common/http';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import type { Genre, GenreDetails } from './types/genre';

export async function all(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.genres}`;
    const response = await http.get<Genre[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.genre.namesBadRequest);
    throw error;
  }
}

export async function details(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.genres}/${id}`;
    const response = await http.get<GenreDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.genre.details);
    throw error;
  }
}
