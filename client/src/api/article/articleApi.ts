import axios from 'axios';
import { baseUrl, baseAdminUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import type { Article, ArticleInput } from './types/article.type';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';

export async function details(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseUrl}${routes.article}/${id}`;
    const response = await axios.get<Article>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.get);
  }
}

export async function create(article: ArticleInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.article}`;
    const response = await axios.post<{ id: number }>(url, article, getAuthConfig(token, signal));

    return response.data.id;
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.create);
  }
}

export async function edit(id: number, article: ArticleInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.article}/${id}`;
    await axios.put(url, article, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.edit);
  }
}

export async function remove(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${baseAdminUrl}${routes.article}/${id}`;
    await axios.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.delete);
  }
}
