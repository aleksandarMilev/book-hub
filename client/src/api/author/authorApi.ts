import axios from 'axios';
import { baseUrl, baseAdminUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import type { Author, AuthorInput, AuthorDetails } from './types/author.type';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';

export async function details(id: string, token: string, isAdmin: boolean, signal?: AbortSignal) {
  try {
    const url = `${isAdmin ? baseAdminUrl : baseUrl}${routes.author}/${id}`;
    const response = await axios.get<AuthorDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.notFound);
  }
}

export async function create(author: AuthorInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.author}`;
    const response = await axios.post<{ id: string }>(url, author, getAuthConfig(token, signal));

    return response.data.id;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.create);
  }
}

export async function edit(id: number, author: AuthorInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.author}/${id}`;
    await axios.put(url, author, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.edit);
  }
}

export async function remove(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.author}/${id}`;
    await axios.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.delete);
  }
}

export async function approve(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.author}/${id}/approve`;
    await axios.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.approve);
  }
}

export async function reject(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.author}/${id}/reject`;
    await axios.patch<void>(url, null, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.reject);
  }
}

export async function names(token: string, signal?: AbortSignal) {
  try {
    const url = `${baseUrl}${routes.authorNames}`;
    const response = await axios.get<Author[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.namesBadRequest);
  }
}

export async function topThree(token: string, signal?: AbortSignal) {
  try {
    const url = `${baseUrl}${routes.topThreeAuthors}`;
    const response = await axios.get<Author[]>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.author.topThree);
  }
}
