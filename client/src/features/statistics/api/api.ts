import type { Statistics } from '@/features/statistics/types/statistics';
import { http, processError } from '@/shared/api/http';
import { baseUrl, routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export const all = async () => {
  try {
    const url = `${baseUrl}${routes.statistics}`;
    const { data } = await http.get<Statistics>(url);

    return data;
  } catch (error) {
    processError(error, errors.statistics.all);
  }
};


