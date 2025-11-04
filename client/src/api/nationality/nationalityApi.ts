import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig } from '../common/utils';
import type { Nationality } from './types/nationality';

export async function all(token: string) {
  try {
    const url = `${baseUrl}${routes.authorNationalities}`;
    const response = await axios.get<Nationality[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.nationality.namesBadRequest);
  }
}
