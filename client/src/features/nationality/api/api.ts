import type { Nationality } from '@/features/nationality/types/nationality';
import { getAuthConfig, http, processError } from '@/shared/api/http';
import { baseUrl, routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export async function all(token: string, signal?: AbortSignal) {
  try {
    const url = `${baseUrl}${routes.authorNationalities}`;
    const response = await http.get<Nationality[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.nationality.all);
  }
}
