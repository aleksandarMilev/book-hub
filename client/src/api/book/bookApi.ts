import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { pagination } from '../../common/constants/defaultValues';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import { http, httpAdmin } from '../common/http';
import type { PagedResult } from '../common/types/pagedResults';
import type { BookDetailsResponse, BookListItemType, BookUpsertPayload } from './types/book';

export async function topThree(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.topThreeBooks}`;
    const response = await http.get<BookListItemType[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.topThree);
    throw error;
  }
}

export async function byGenre(
  token: string,
  genreId: string | number,
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.booksByGenre}/${encodeURIComponent(String(genreId))}?page=${page}&pageSize=${pageSize}`;
    const response = await http.get<PagedResult<BookListItemType>>(
      url,
      getAuthConfig(token, signal),
    );

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.search.badRequest);
    throw error;
  }
}

export async function byAuthor(
  token: string,
  authorId: string | number,
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.booksByAuthor}/${encodeURIComponent(String(authorId))}?page=${page}&pageSize=${pageSize}`;
    const response = await http.get<PagedResult<BookListItemType>>(
      url,
      getAuthConfig(token, signal),
    );

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.search.badRequest);
    throw error;
  }
}

export async function details(id: number, token: string, isAdmin: boolean, signal?: AbortSignal) {
  try {
    const url = `${routes.book}/${id}`;
    const httpClient = isAdmin ? httpAdmin : http;
    const response = await httpClient.get<BookDetailsResponse>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.notFound);
    throw error;
  }
}

export async function create(book: BookUpsertPayload, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.book}`;
    const response = await httpAdmin.post<{ id: number }>(url, book, getAuthConfig(token, signal));

    return response.data.id;
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.create);
    throw error;
  }
}

export async function edit(
  id: number,
  book: BookUpsertPayload,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.book}/${id}`;
    await httpAdmin.put(url, book, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.edit);
    throw error;
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.book}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.delete);
    throw error;
  }
}

export async function approve(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.book}/${id}/approve`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.approve);
    throw error;
  }
}

export async function reject(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.book}/${id}/reject`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.book.reject);
    throw error;
  }
}
