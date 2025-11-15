import type { ReadingStatusUI } from '@/features/reading-list/types/readingList.js';

export const getTotalPages = (totalItems: number, pageSize: number) =>
  Math.max(1, Math.ceil(totalItems / pageSize));

export const getTitle = (statusUI: ReadingStatusUI, firstName: string) =>
  statusUI === 'read'
    ? `${firstName} · Read`
    : statusUI === 'to read'
      ? `${firstName} · To Read`
      : `${firstName} · Currently Reading`;
