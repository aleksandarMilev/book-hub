import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/search/api/api.js';
import type {
  ArticlesSearchResult,
  AuthorsSearchResult,
  BooksSearchResult,
  ChatsSearchResult,
  ProfilesSearchResult,
} from '@/features/search/types/search.js';
import { routes } from '@/shared/lib/constants/api.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import type { PaginatedResult } from '@/shared/types/paginatedResult.js';

function useSearch<T>(
  search: (
    searchTerm: string,
    page: number,
    pageSize: number,
    token?: string,
    signal?: AbortSignal,
  ) => Promise<PaginatedResult<T>>,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [items, setItems] = useState<T[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      try {
        setIsFetching(true);

        const data = await search(searchTerm || '', page, pageSize, token, signal);

        setItems(data.items ?? []);
        setTotalItems(data.totalItems ?? 0);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load search results.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, search, searchTerm, page, pageSize, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { items, totalItems, isFetching, refetch: fetchData };
}

export function useSearchBooks(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<BooksSearchResult>(api.searchBooks, searchTerm, page, pageSize);
}

export function useSearchChats(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<ChatsSearchResult>(api.searchChats, searchTerm, page, pageSize);
}

export function useSearchAuthors(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<AuthorsSearchResult>(api.searchAuthors, searchTerm, page, pageSize);
}

export function useSearchProfiles(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<ProfilesSearchResult>(api.searchProfiles, searchTerm, page, pageSize);
}

export function useSearchArticles(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<ArticlesSearchResult>(api.searchArticles, searchTerm, page, pageSize);
}
