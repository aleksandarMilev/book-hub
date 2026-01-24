import {
  getAuthConfig,
  getAuthConfigForFile,
  getPublicConfig,
  http,
  httpAdmin,
  processError,
} from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';
import type { PaginatedResult } from '@/shared/types/paginatedResult.js';

import type { Book, BookDetails, CreateBook } from '../types/book.js';

export const topThree = async (signal?: AbortSignal) => {
  try {
    const url = `${routes.topThreeBooks}`;
    const { data } = await http.get<Book[]>(url, getPublicConfig(signal));

    return data;
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
  authorId: string,
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
  id: string,
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
    const formData = new FormData();

    writeFormData(formData, book);

    const { data } = await http.post<BookDetails>(
      url,
      formData,
      getAuthConfigForFile(token, signal),
    );

    return data;
  } catch (error) {
    processError(error, errors.book.create);
  }
};

export const edit = async (id: string, book: CreateBook, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}`;
    const formData = new FormData();

    writeFormData(formData, book);

    await http.put(url, formData, getAuthConfigForFile(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.book.edit);
  }
};

export const remove = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.book.delete);
  }
};

export const approve = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}/approve`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

export const reject = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.book}/${id}/reject`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

const writeFormData = (formData: FormData, book: CreateBook) => {
  formData.append('title', book.title);
  formData.append('shortDescription', book.shortDescription);
  formData.append('longDescription', book.longDescription);

  if (book.authorId) {
    formData.append('authorId', String(book.authorId));
  }

  if (book.publishedDate) {
    formData.append('publishedDate', book.publishedDate);
  }

  if (book.image) {
    formData.append('image', book.image);
  }

  if (book.genres && book.genres.length > 0) {
    book.genres.forEach((g) => formData.append('genres', String(g)));
  }
};
