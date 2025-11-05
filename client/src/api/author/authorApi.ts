import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import { http, httpAdmin } from '../common/http';
import type { Author, AuthorDetails, AuthorInput, AuthorName } from './types/author';

export async function names(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.authorNames}`;
    const response = await http.get<AuthorName[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.namesBadRequest);
    throw error;
  }
}

export async function topThree(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.topThreeAuthors}`;
    const response = await http.get<Author[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.topThree);
    throw error;
  }
}

export async function details(id: number, token: string, isAdmin: boolean, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    const httpClient = isAdmin ? httpAdmin : http;
    const response = await httpClient.get<AuthorDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.notFound);
    throw error;
  }
}

export async function create(author: AuthorInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}`;
    const response = await httpAdmin.post<{ id: number }>(
      url,
      author,
      getAuthConfig(token, signal),
    );

    return response.data.id;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.create);
    throw error;
  }
}

export async function edit(id: number, author: AuthorInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    await httpAdmin.put(url, author, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.edit);
    throw error;
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.delete);
    throw error;
  }
}

export async function approve(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}/approve`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.approve);
    throw error;
  }
}

export async function reject(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}/reject`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.reject);
    throw error;
  }
}
