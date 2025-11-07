import type { Article, ArticleInput } from './types/article';

import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';

import { http, httpAdmin } from '../common/http';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';


export async function details(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}/${id}`;
    const response = await http.get<Article>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.get);
    throw error;
  }
}

export async function create(article: ArticleInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}`;
    const response = await httpAdmin.post<{ id: number }>(
      url,
      article,
      getAuthConfig(token, signal),
    );

    return response.data.id;
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.create);
    throw error;
  }
}

export async function edit(id: number, article: ArticleInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.put(url, article, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.edit);
    throw error;
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.article.delete);
    throw error;
  }
}
