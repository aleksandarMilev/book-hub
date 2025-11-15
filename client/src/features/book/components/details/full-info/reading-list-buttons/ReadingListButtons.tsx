import { type FC, useState } from 'react';

import { useListActions } from '@/features/reading-list/hooks/useCrud.js';
import type { ReadingStatusUI } from '@/features/reading-list/types/readingList.js';

export const ReadingListButtons: FC<{
  bookId: number;
  initialReadingStatus: ReadingStatusUI;
  token: string;
  showMessage: (message: string, success?: boolean) => void;
}> = ({ bookId, initialReadingStatus, token, showMessage }) => {
  const [readingStatus, setReadingStatus] = useState<ReadingStatusUI>(initialReadingStatus);
  const { addToList, removeFromList } = useListActions(bookId, token, showMessage);

  const onAdd = async (status: ReadingStatusUI) => {
    const success = await addToList(status);
    if (success) {
      setReadingStatus(status);
    }
  };

  const onRemove = async () => {
    const success = await removeFromList(readingStatus);
    if (success) {
      setReadingStatus(null);
    }
  };

  if (!readingStatus) {
    return (
      <div className="d-flex gap-2">
        <button className="btn btn-outline-success" onClick={() => onAdd('read')}>
          Mark as Read
        </button>
        <button className="btn btn-outline-primary" onClick={() => onAdd('to read')}>
          Add to Want to Read
        </button>
        <button className="btn btn-outline-warning" onClick={() => onAdd('currently reading')}>
          Add to Currently Reading
        </button>
      </div>
    );
  }

  return (
    <div className="reading-status">
      {readingStatus === 'read' && (
        <p>
          You marked this book as <strong>Read</strong>.
        </p>
      )}
      {readingStatus === 'to read' && (
        <p>
          You added this book to <strong>Want to Read</strong>.
        </p>
      )}
      {readingStatus === 'currently reading' && (
        <p>
          You are currently <strong>Reading</strong> this book.
        </p>
      )}
      <button className="btn btn-outline-danger" onClick={onRemove}>
        Remove from List
      </button>
    </div>
  );
};
