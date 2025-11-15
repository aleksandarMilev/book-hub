import type { Book } from '@/features/book/types/book.js';
import type { ReadingStatusAPI } from '@/features/reading-list/types/readingList.js';
import { getAuthConfig, http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import type { PaginatedResult } from '@/shared/types/paginatedResult.js';

export async function get(
  userId: string,
  token: string,
  status: ReadingStatusAPI,
  pageIndex?: number | null,
  pageSize?: number | null,
  signal?: AbortSignal,
) {
  try {
    const params: Record<string, string | number | null> = { userId, status };
    if (pageIndex != null) {
      params.pageIndex = pageIndex;
    }

    if (pageSize != null) {
      params.pageSize = pageSize;
    }

    const url = `${routes.readingList}`;
    const response = await http.get<PaginatedResult<Book>>(url, {
      ...getAuthConfig(token, signal),
      params,
    });

    return response.data;
  } catch (error) {
    processError(error, errors.readingList.all);
  }
}

export async function add(
  bookId: number,
  status: ReadingStatusAPI,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.readingList}`;
    await http.post(url, { bookId, status }, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.readingList.add);
  }
}

export async function remove(
  bookId: number,
  status: ReadingStatusAPI,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.readingList}`;
    await http.delete(url, { ...getAuthConfig(token, signal), data: { bookId, status } });

    return true;
  } catch (error) {
    processError(error, errors.readingList.remove);
  }
}
