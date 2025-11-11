import axios from 'axios';

import type {
  ArticlesSearchResult,
  AuthorsSearchResult,
  BooksSearchResult,
  ChatsSearchResult,
  ProfilesSearchResult,
} from '@/features/search/types/search';
import { getAuthConfig, processError } from '@/shared/api/http';
import { baseUrl, routes } from '@/shared/lib/constants/api';
import { pagination } from '@/shared/lib/constants/defaultValues';
import { errors } from '@/shared/lib/constants/errorMessages';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

async function search<T>(
  route: string,
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
): Promise<PaginatedResult<T>> {
  try {
    const url = `${baseUrl}${route}`;
    const params = { searchTerm, page, pageSize };
    const response = await axios.get<PaginatedResult<T>>(url, {
      ...getAuthConfig(token, signal),
      params,
    });

    return response.data;
  } catch (error) {
    processError(error, errors.search.all);
  }
}

export async function searchBooks(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<BooksSearchResult>(routes.searchBooks, token, searchTerm, page, pageSize, signal);
}

export async function searchChats(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<ChatsSearchResult>(routes.searchChats, token, searchTerm, page, pageSize, signal);
}

export async function searchAuthors(
  token: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  signal?: AbortSignal,
) {
  return search<AuthorsSearchResult>(
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
  return search<ProfilesSearchResult>(
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
  return search<ArticlesSearchResult>(
    routes.searchArticles,
    token,
    searchTerm,
    page,
    pageSize,
    signal,
  );
}
