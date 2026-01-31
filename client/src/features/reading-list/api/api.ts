import type { Book } from '@/features/book/types/book';
import type { ReadingStatusAPI } from '@/features/reading-list/types/readingList';
import { getAuthConfig, http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

export const getLastCurrentlyReading = async (
  userId: string,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const params: Record<string, string | number> = { userId };
    const url = `${routes.lastCurrentlyReading}`;

    const { data } = await http.get<Book>(url, {
      ...getAuthConfig(token, signal),
      params,
    });

    return data;
  } catch (error) {
    processError(error, errors.readingList.lastCurrentlyReading);
  }
};

export const getList = async (
  userId: string,
  token: string,
  status: ReadingStatusAPI,
  pageIndex?: number | null,
  pageSize?: number | null,
  signal?: AbortSignal,
) => {
  try {
    const params: Record<string, string | number> = { userId, status };
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
};

export const add = async (
  bookId: string,
  status: ReadingStatusAPI,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.readingList}`;
    await http.post(url, { bookId, status }, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.readingList.add);
  }
};

export const remove = async (
  bookId: string,
  status: ReadingStatusAPI,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.readingList}`;
    await http.delete(url, { ...getAuthConfig(token, signal), data: { bookId, status } });

    return true;
  } catch (error) {
    processError(error, errors.readingList.remove);
  }
};


