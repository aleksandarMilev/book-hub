import './ReadingListButtons.css';

import { type FC, useState } from 'react';

import { useListActions } from '@/features/reading-list/hooks/useCrud.js';
import type { ReadingStatusUI } from '@/features/reading-list/types/readingList.js';

type Props = {
  bookId: string;
  initialReadingStatus: ReadingStatusUI;
  token: string;
  showMessage: (message: string, success?: boolean) => void;
};

export const ReadingListButtons: FC<Props> = ({
  bookId,
  initialReadingStatus,
  token,
  showMessage,
}) => {
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
      <div className="book-reading-list-buttons">
        <button
          className="btn btn-outline-success book-reading-list-button"
          type="button"
          onClick={() => onAdd('read')}
        >
          Mark as Read
        </button>
        <button
          className="btn btn-outline-primary book-reading-list-button"
          type="button"
          onClick={() => onAdd('to read')}
        >
          Add to Want to Read
        </button>
        <button
          className="btn btn-outline-warning book-reading-list-button"
          type="button"
          onClick={() => onAdd('currently reading')}
        >
          Add to Currently Reading
        </button>
      </div>
    );
  }

  return (
    <div className="book-reading-status">
      {readingStatus === 'read' && (
        <p className="book-reading-status-text">
          You marked this book as <strong>Read</strong>.
        </p>
      )}
      {readingStatus === 'to read' && (
        <p className="book-reading-status-text">
          You added this book to <strong>Want to Read</strong>.
        </p>
      )}
      {readingStatus === 'currently reading' && (
        <p className="book-reading-status-text">
          You are currently <strong>Reading</strong> this book.
        </p>
      )}
      <button
        className="btn btn-outline-danger book-reading-remove-button"
        type="button"
        onClick={onRemove}
      >
        Remove from List
      </button>
    </div>
  );
};
