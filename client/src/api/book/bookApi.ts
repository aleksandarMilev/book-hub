import axios from 'axios';
import { getAuthConfig } from '../common/utils';
import { baseAdminUrl, baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { pagination } from '../../common/constants/defaultValues';
import type { Book } from './types/book.type';

export async function topThree(token: string): Promise<Book[]> {
  try {
    const url = `${baseUrl}${routes.topThreeBooks}`;
    const response = await axios.get<Book[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.book.topThree);
  }
}

export async function byGenre(
  token: string,
  genreId: string,
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
) {
  try {
    const url = `${baseUrl}${routes.booksByGenre}/${encodeURIComponent(
      genreId,
    )}?page=${page}&pageSize=${pageSize}`;

    const response = await axios.get<{ items: Book[]; totalItems: number }>(
      url,
      getAuthConfig(token),
    );

    return response.data;
  } catch {
    throw new Error(errors.search.badRequest);
  }
}

export async function byAuthor(
  token: string,
  authorId: string,
  page = pagination.defaultPageIndex,
  pageSize = pagination.defaultPageSize,
) {
  try {
    const url = `${baseUrl}${routes.booksByAuthor}/${encodeURIComponent(
      authorId,
    )}?page=${page}&pageSize=${pageSize}`;

    const response = await axios.get<{ items: Book[]; totalItems: number }>(
      url,
      getAuthConfig(token),
    );

    return response.data;
  } catch {
    throw new Error(errors.search.badRequest);
  }
}

export async function details(id: string, token: string, isAdmin: boolean) {
  try {
    const url = `${isAdmin ? baseAdminUrl : baseUrl}${routes.book}/${id}`;
    const response = await axios.get<Book>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.book.notfound);
  }
}

export async function create(book: Book, token: string) {
  try {
    const url = `${baseUrl}${routes.book}`;
    const response = await axios.post<{ id: string }>(url, book, getAuthConfig(token));

    return response.data.id;
  } catch {
    throw new Error(errors.book.create);
  }
}

export async function edit(id: number, book: Book, token: string) {
  try {
    const url = `${baseUrl}${routes.book}/${id}`;
    await axios.put(url, book, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.book.edit);
  }
}

export async function remove(id: number, token: string) {
  try {
    const url = `${baseUrl}${routes.book}/${id}`;
    await axios.delete(url, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.book.delete);
  }
}

export async function approve(id: number, token: string) {
  try {
    const url = `${baseAdminUrl}${routes.book}/${id}/approve`;
    await axios.patch(url, null, getAuthConfig(token));
  } catch {
    throw new Error(errors.book.approve);
  }
}

export async function reject(id: number, token: string) {
  try {
    const url = `${baseAdminUrl}${routes.book}/${id}/reject`;
    await axios.patch(url, null, getAuthConfig(token));
  } catch {
    throw new Error(errors.book.reject);
  }
}
