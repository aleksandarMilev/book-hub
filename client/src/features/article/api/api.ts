import type { ArticleDetails, CreateArticle } from '@/features/article/types/article.js';
import {
    getAuthConfig,
    getAuthConfigForFile,
    http,
    httpAdmin,
    processError,
} from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export const details = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.articles}/${id}`;
    const { data } = await http.get<ArticleDetails>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.article.byId);
  }
};

export const detailsForEdit = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.articles}/${id}`;
    const { data } = await httpAdmin.get<ArticleDetails>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.article.byId);
  }
};

export const create = async (article: CreateArticle, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.articles}`;
    const formData = new FormData();

    writeFormData(formData, article);

    const { data } = await httpAdmin.post<{ id: string }>(
      url,
      formData,
      getAuthConfigForFile(token, signal),
    );

    return data.id;
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
    const url = `${routes.articles}/${id}`;
    const formData = new FormData();

    writeFormData(formData, article);

    await httpAdmin.put(url, formData, getAuthConfigForFile(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.article.edit);
  }
};

export const remove = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.articles}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.article.delete);
  }
};

const writeFormData = (formData: FormData, article: CreateArticle) => {
  formData.append('title', article.title);
  formData.append('introduction', article.introduction);
  formData.append('content', article.content);

  if (article.image) {
    formData.append('image', article.image);
  }
};
