import { useState } from 'react';
import { useLocation } from 'react-router-dom';

import { useList } from '@/features/reading-list/hooks/useCrud';
import type { ReadingStatusUI } from '@/features/reading-list/types/readingList';
import { pagination } from '@/shared/lib/constants/defaultValues';

export const useReadingListPage = () => {
  const location = useLocation();
  const state = location.state as
    | {
        id: string;
        readingListStatus: ReadingStatusUI;
        firstName: string;
      }
    | undefined;

  const ownerId = state?.id;
  const statusUI: ReadingStatusUI = state?.readingListStatus ?? 'read';
  const firstName = state?.firstName ?? 'User';

  const missing = !state?.id || !state?.readingListStatus || !state?.firstName;

  const [page, setPage] = useState<number>(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const { readingList, totalItems, isFetching } = useList(
    statusUI,
    page,
    pageSize,
    false,
    ownerId,
    missing,
  );

  return {
    missing,
    isFetching,
    totalItems,
    pageSize,
    setPage,
    page,
    statusUI,
    firstName,
    readingList,
  };
};


