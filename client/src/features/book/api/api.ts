import { getAuthConfig, http, httpAdmin, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';
import type { PaginatedResult } from '@/shared/types/paginatedResult.js';

import type { Book, BookDetails, CreateBook } from '../types/book.js';

export const topThree = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.topThreeBooks}`;
    const response = await http.get<Book[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.book.topThree);
  }
};

export const byGenre = async (
  token: string,
  genreId: string | number,
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.booksByGenre}/${encodeURIComponent(String(genreId))}?page=${page}&pageSize=${pageSize}`;
    const response = await http.get<PaginatedResult<Book>>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.book.all);
  }
};

export const byAuthor = async (
  token: string,
  authorId: string | number,
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.booksByAuthor}/${encodeURIComponent(String(authorId))}?page=${page}&pageSize=${pageSize}`;
    const response = await http.get<PaginatedResult<Book>>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.book.all);
  }
};

export const details = async (
  id: number,
  token: string,
  isAdmin: boolean,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.book}/${id}`;
    const httpClient = isAdmin ? httpAdmin : http;
    const response = await httpClient.get<BookDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.book.byId);
  }
};

export const create = async (book: CreateBook, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}`;
    const response = await httpAdmin.post<{ id: number }>(url, book, getAuthConfig(token, signal));

    return response.data.id;
  } catch (error) {
    processError(error, errors.book.create);
  }
};

export const edit = async (id: number, book: CreateBook, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}`;
    await httpAdmin.put(url, book, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.book.edit);
  }
};

export const remove = async (id: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.book.delete);
  }
};

export const approve = async (id: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}/approve`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

export const reject = async (id: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}/reject`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};
