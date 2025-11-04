import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig } from '../common/utils';
import type { GenreName } from './types/genreName';
import type { GenreDetails } from './types/genreDetails';

export async function all(token: string) {
  try {
    const url = `${baseUrl}${routes.genres}`;
    const response = await axios.get<GenreName[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.genre.namesBadRequest);
  }
}

export async function details(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.genres}/${id}`;
    const response = await axios.get<GenreDetails>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.genre.details);
  }
}
