import axios, { HttpStatusCode } from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig } from '../common/utils';
import type { PagedReadingList } from './types/pagedReadingList';
import type { ReadingListError } from './types/readingListError';

export async function get(
  userId: string,
  token: string,
  status: string,
  page?: number | null,
  pageSize?: number | null,
) {
  try {
    const params: Record<string, string | number> = { userId, status };
    if (page) {
      params.pageIndex = page;
    }

    if (pageSize) {
      params.pageSize = pageSize;
    }

    const url = `${baseUrl}${routes.readingList}`;
    const response = await axios.get<PagedReadingList>(url, {
      ...getAuthConfig(token),
      params,
    });

    return response.data;
  } catch {
    throw new Error(errors.readingList.currentlyReading);
  }
}

export async function add(bookId: number, status: string, token: string) {
  try {
    const url = `${baseUrl}${routes.readingList}`;
    await axios.post(url, { bookId, status }, getAuthConfig(token));

    return null;
  } catch (error: any) {
    if (error.response?.status === HttpStatusCode.BadRequest && error.response.data) {
      return error.response.data as ReadingListError;
    }

    throw new Error(errors.readingList.add);
  }
}

export async function remove(bookId: number, status: string, token: string) {
  try {
    const url = `${baseUrl}${routes.readingList}`;
    await axios.delete(url, {
      ...getAuthConfig(token),
      data: { bookId, status },
    });
  } catch {
    throw new Error(errors.readingList.add);
  }
}
