import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import { http, httpAdmin } from '@/shared/api/http';
import { getAuthConfig, handleRequestError } from '@/shared/api/utils';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';

export async function details(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}/${id}`;
    const response = await http.get<ArticleDetails>(url, getAuthConfig(token, signal));

    return response.data;
  } catch (error) {
    handleRequestError(error, errors.article.get);
  }
}

export async function create(article: CreateArticle, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}`;
    const response = await httpAdmin.post<{ id: number }>(
      url,
      article,
      getAuthConfig(token, signal),
    );

    return response.data.id;
  } catch (error) {
    handleRequestError(error, errors.article.create);
  }
}

export async function edit(
  id: number,
  article: CreateArticle,
  token: string,
  signal?: AbortSignal,
) {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.put(url, article, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    handleRequestError(error, errors.article.edit);
  }
}

export async function remove(id: number, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.article}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    handleRequestError(error, errors.article.delete);
  }
}
