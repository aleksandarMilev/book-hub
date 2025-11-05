import axios from 'axios';
import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { pagination } from '../../common/constants/defaultValues';
import type { PagedResult } from '../common/types/pagedResults';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import type { BookSearchResult } from './types/bookSearchResult';
import type { ChatSearchResult } from './types/chatResultSearch';
import type { AuthorSearchResult } from './types/authorSearchResult';
import type { ProfileSearchResult } from './types/profileSearchResult';
import type { ArticleSummary } from '../article/types/article';

async function search<T>(
  route: string,
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
): Promise<PagedResult<T> | undefined> {
  try {
    const url = `${baseUrl}${route}`;
    const params = { searchTerm, page, pageSize };

    const response = await axios.get<PagedResult<T>>(url, {
      ...getAuthConfig(token, signal),
      params,
    });

    return response.data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.search.badRequest);
  }
}

export async function searchBooks(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<BookSearchResult>(routes.searchBooks, token, searchTerm, page, pageSize, signal);
}

export async function searchChats(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<ChatSearchResult>(routes.searchChats, token, searchTerm, page, pageSize, signal);
}

export async function searchAuthors(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<AuthorSearchResult>(
    routes.searchAuthors,
    token,
    searchTerm,
    page,
    pageSize,
    signal,
  );
}

export async function searchProfiles(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<ProfileSearchResult>(
    routes.searchProfiles,
    token,
    searchTerm,
    page,
    pageSize,
    signal,
  );
}

export async function searchArticles(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<ArticleSummary>(routes.searchArticles, token, searchTerm, page, pageSize, signal);
}
