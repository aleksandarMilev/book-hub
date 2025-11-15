import type { Nationality } from '@/features/nationality/types/nationality.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { baseUrl, routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export async function all(token: string, signal?: AbortSignal) {
  try {
    const url = `${baseUrl}${routes.authorNationalities}`;
    const response = await http.get<Nationality[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.nationality.all);
  }
}
