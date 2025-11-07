import { HttpStatusCode, isAxiosError } from 'axios';

import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';

import { http } from '../common/http';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';

export async function get(
  userId: string,
  token: string,
  status: string,
  page?: number | null,
  pageSize?: number | null,
  signal?: AbortSignal,
) {
  try {
    const params: Record<string, string | number> = { userId, status };

    if (page != null) {
      params.page = page;
      params.pageIndex = page;
    }

    if (pageSize != null) {
      params.pageSize = pageSize;
    }

    const url = `${routes.readingList}`;
    const response = await http.get<any>(url, {
      ...getAuthConfig(token, signal),
      params,
    });

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.readingList.currentlyReading);
    throw error;
  }
}

export async function add(bookId: number, status: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.readingList}`;
    await http.post(url, { bookId, status }, getAuthConfig(token, signal));

    return null;
  } catch (error: unknown) {
    try {
      returnIfRequestCanceled(error, errors.readingList.add);
    } catch (error) {
      throw error;
    }

    if (
      isAxiosError(error) &&
      error?.response?.status === HttpStatusCode.BadRequest &&
      error.response.data
    ) {
      return error.response.data;
    }

    throw new Error(errors.readingList.add);
  }
}

export async function remove(bookId: number, status: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.readingList}`;
    await http.delete(url, {
      ...getAuthConfig(token, signal),
      data: { bookId, status },
    });
  } catch (error) {
    returnIfRequestCanceled(error, errors.readingList.remove);
    throw error;
  }
}
