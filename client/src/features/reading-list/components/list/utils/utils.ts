import type { TFunction } from 'i18next';

import type { ReadingStatusUI } from '@/features/reading-list/types/readingList';

export const getTotalPages = (totalItems: number, pageSize: number) =>
  Math.max(1, Math.ceil(totalItems / pageSize));

export const getTitle = (t: TFunction, statusUI: ReadingStatusUI, firstName: string) => {
  const statusText =
    statusUI === 'read'
      ? t('status.read')
      : statusUI === 'to read'
        ? t('status.toRead')
        : t('status.currentlyReading');

  return `${firstName} Â· ${statusText}`;
};


