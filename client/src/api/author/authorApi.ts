import axios from 'axios';
import { getAuthConfig } from '../common/utils';
import type { Author } from './types/author.type';
import { baseAdminUrl, baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';

export async function topThree(token: string) {
  try {
    const url = `${baseUrl}${routes.topThreeAuthors}`;
    const response = await axios.get<Author[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.author.topThree);
  }
}

export async function names(token: string): Promise<Author[]> {
  try {
    const url = `${baseUrl}${routes.authorNames}`;
    const response = await axios.get<Author[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.author.namesBadRequest);
  }
}

export async function details(id: number, token: string, isAdmin: boolean) {
  try {
    const url = `${isAdmin ? baseAdminUrl : baseUrl}${routes.author}/${id}`;
    const response = await axios.get<Author>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.author.notfound);
  }
}

export async function create(author: Author, token: string): Promise<string> {
  try {
    const url = `${baseUrl}${routes.author}`;
    const response = await axios.post<{ id: string }>(url, author, getAuthConfig(token));

    return response.data.id;
  } catch {
    throw new Error(errors.author.create);
  }
}

export async function edit(id: number, author: Author, token: string): Promise<boolean> {
  try {
    const url = `${baseUrl}${routes.author}/${id}`;
    await axios.put(url, author, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.author.edit);
  }
}

export async function remove(id: number, token: string): Promise<boolean> {
  try {
    const url = `${baseUrl}${routes.author}/${id}`;
    await axios.delete(url, getAuthConfig(token));

    return true;
  } catch {
    throw new Error(errors.author.delete);
  }
}

export async function approve(id: number, token: string): Promise<void> {
  try {
    const url = `${baseAdminUrl}${routes.author}/${id}/approve`;
    await axios.patch(url, null, getAuthConfig(token));
  } catch {
    throw new Error(errors.author.approve);
  }
}

export async function reject(id: number, token: string): Promise<void> {
  try {
    const url = `${baseAdminUrl}${routes.author}/${id}/reject`;
    await axios.patch(url, null, getAuthConfig(token));
  } catch {
    throw new Error(errors.author.reject);
  }
}
