import type { ArticleDetails, CreateArticle } from '@/features/article/types/article.js';
import { getAuthConfig, http, httpAdmin, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export const details = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.article}/${id}`;
    const response = await http.get<ArticleDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    processError(error, errors.article.byId);
  }
};

export const create = async (article: CreateArticle, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.article}`;
    const response = await httpAdmin.post<{ id: string }>(
      url,
      article,
      getAuthConfig(token, signal),
    );

    return response.data.id;
  } catch (error) {
    processError(error, errors.article.create);
  }
};

export const edit = async (
  id: string,
  article: CreateArticle,
  token: string,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.put(url, article, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.article.edit);
  }
};

export const remove = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.article.delete);
  }
};
