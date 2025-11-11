import { type ChangeEvent, useState } from 'react';

import * as searchHooks from '@/hooks/useSearch';
import { useDebounce } from '@/shared/hooks/useDebounce';
import { pagination } from '@/shared/lib/constants/defaultValues';

export const useListPage = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const {
    items: articles,
    totalItems,
    isFetching,
  } = searchHooks.useSearchArticles(debouncedSearchTerm, page, pageSize);

  const totalPages = Math.ceil(totalItems / pageSize) || 1;

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    setPage(pagination.defaultPageIndex);
  };

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) {
      return;
    }

    setPage(newPage);
  };

  return {
    articles,
    isFetching,
    searchTerm,
    setSearchTerm,
    page,
    totalPages,
    handleSearchChange,
    handlePageChange,
  };
};
