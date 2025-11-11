import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import { getAuthConfig, http, httpAdmin, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export const details = async (id: number, token: string, signal?: AbortSignal) => {
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
    const response = await httpAdmin.post<{ id: number }>(
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
  id: number,
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

export const remove = async (id: number, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.article.delete);
  }
};
