import type {
  Author,
  AuthorDetails,
  AuthorNames,
  CreateAuthor,
} from '@/features/author/types/author.js';
import {
  getAuthConfig,
  getPublicConfig,
  http,
  httpAdmin,
  processError,
} from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';

export async function names(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.authorNames}`;
    const response = await http.get<AuthorNames[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.author.all);
  }
}

export const topThree = async (signal?: AbortSignal) => {
  try {
    const url = `${routes.topThreeAuthors}`;
    const { data } = await http.get<Author[]>(url, getPublicConfig(signal));

    return data;
  } catch (error) {
    processError(error, errors.author.topThree);
  }
};

export async function details(id: number, token: string, isAdmin: boolean, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    const httpClient = isAdmin ? httpAdmin : http;
    const response = await httpClient.get<AuthorDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.author.byId);
  }
}

export async function create(author: CreateAuthor, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}`;
    const response = await http.post<{ id: number }>(url, author, getAuthConfig(token, signal));

    return response.data.id;
  } catch (error) {
    processError(error, errors.author.create);
  }
}

export async function edit(id: number, author: CreateAuthor, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    await http.put(url, author, getAuthConfig(token, signal));
  } catch (error) {
    processError(error, errors.author.edit);
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.author.delete);
  }
}

export async function approve(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}/approve`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
}

export async function reject(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.author}/${id}/reject`;
    await httpAdmin.patch<void>(url, null, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, baseErrors.general);
  }
}
