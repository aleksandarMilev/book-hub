import type { Statistics } from '@/features/statistics/types/statistics.js';
import { http, processError } from '@/shared/api/http.js';
import { baseUrl, routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export async function all() {
  try {
    const url = `${baseUrl}${routes.statistics}`;
    const response = await http.get<Statistics>(url);

    return response.data;
  } catch (error) {
    processError(error, errors.statistics.all);
  }
}
