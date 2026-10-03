import type {
  ArticlesSearchResult,
  AuthorsSearchResult,
  BooksSearchResult,
  GenresSearchResult,
  ProfilesSearchResult,
} from '@/features/search/types/search';
import { getAuthConfig, getPublicConfig, http, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { pagination } from '@/shared/lib/constants/defaultValues';
import { errors } from '@/shared/lib/constants/errorMessages';
import type { PaginatedResult } from '@/shared/types/paginatedResult';

async function search<T>(
  route: string,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  token?: string,
  signal?: AbortSignal,
): Promise<PaginatedResult<T>> {
  try {
    // Through `http`, so the 401 interceptor and the shared base URL apply (F-16).
    const params = { searchTerm, page, pageSize };
    const options = {
      ...(token ? getAuthConfig(token, signal) : getPublicConfig(signal)),
      params,
    };

    const response = await http.get<PaginatedResult<T>>(route, options);

    return response.data;
  } catch (error) {
    return processError(error, errors.search.all);
  }
}

export async function searchGenres(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  token?: string,
  signal?: AbortSignal,
) {
  return search<GenresSearchResult>(routes.searchGenres, searchTerm, page, pageSize, token, signal);
}

export async function searchBooks(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  token?: string,
  signal?: AbortSignal,
) {
  return search<BooksSearchResult>(routes.searchBooks, searchTerm, page, pageSize, token, signal);
}

export async function searchAuthors(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  token?: string,
  signal?: AbortSignal,
) {
  return search<AuthorsSearchResult>(
    routes.searchAuthors,
    searchTerm,
    page,
    pageSize,
    token,
    signal,
  );
}

export async function searchProfiles(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  token?: string,
  signal?: AbortSignal,
) {
  return search<ProfilesSearchResult>(
    routes.searchProfiles,
    searchTerm,
    page,
    pageSize,
    token,
    signal,
  );
}

export async function searchArticles(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
  token?: string,
  signal?: AbortSignal,
) {
  return search<ArticlesSearchResult>(
    routes.searchArticles,
    searchTerm,
    page,
    pageSize,
    token,
    signal,
  );
}
