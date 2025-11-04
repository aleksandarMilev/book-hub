import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/search/searchApi';
import { pagination } from '../common/constants/defaultValues';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { PagedResult } from '../api/common/types/pagedResults';
import type { BookSearchResult } from '../api/search/types/bookSearchResult';
import type { ChatSearchResult } from '../api/search/types/chatResultSearch';
import type { AuthorSearchResult } from '../api/search/types/authorSearchResult';
import type { ProfileSearchResult } from '../api/search/types/profileSearchResult';
import type { ArticleSearchResult } from '../api/search/types/articleSearchResult';

function useSearch<T>(
  search: (
    token: string,
    searchTerm: string,
    page: number,
    pageSize: number,
    signal?: AbortSignal,
  ) => Promise<PagedResult<T> | undefined>,
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [items, setItems] = useState<T[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      try {
        setIsFetching(true);

        const data = await search(token, searchTerm || '', page, pageSize, signal);
        if (!data) {
          return;
        }

        setItems(data.items);
        setTotalItems(data.totalItems);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to load search results.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, searchTerm, page, pageSize, search, navigate],
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
  return useSearch<BookSearchResult>(api.searchBooks, searchTerm, page, pageSize);
}

export function useSearchChats(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<ChatSearchResult>(api.searchChats, searchTerm, page, pageSize);
}

export function useSearchAuthors(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<AuthorSearchResult>(api.searchAuthors, searchTerm, page, pageSize);
}

export function useSearchProfiles(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<ProfileSearchResult>(api.searchProfiles, searchTerm, page, pageSize);
}

export function useSearchArticles(
  searchTerm: string,
  page: number = pagination.defaultPageIndex,
  pageSize: number = pagination.defaultPageSize,
) {
  return useSearch<ArticleSearchResult>(api.searchArticles, searchTerm, page, pageSize);
}
