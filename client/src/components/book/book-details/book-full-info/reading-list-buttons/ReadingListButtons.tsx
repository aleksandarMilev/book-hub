import { useState, type FC } from 'react';

import type { ReadingStatus } from '../../../../../api/readingList/types/readingList';
import { readingListStatus } from '../../../../../common/constants/defaultValues';
import { useListActions } from '../../../../../hooks/useReadingList';


export const ReadingListButtons: FC<{
  bookId: number;
  initialReadingStatus: ReadingStatus;
  token: string;
  showMessage: (msg: string, success?: boolean) => void;
}> = ({ bookId, initialReadingStatus, token, showMessage }) => {
  const [readingStatus, setReadingStatus] = useState<ReadingStatus>(initialReadingStatus);

  const { handleAddToList, handleRemoveFromList } = useListActions(bookId, token, showMessage);

  const onAdd = async (status: ReadingStatus) => {
    const success = await handleAddToList(status);
    if (success) {
      setReadingStatus(status);
    }
  };

  const onRemove = async () => {
    const success = await handleRemoveFromList(readingStatus);
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
        <button className="btn btn-outline-primary" onClick={() => onAdd('toRead')}>
          Add to Want to Read
        </button>
        <button className="btn btn-outline-warning" onClick={() => onAdd('currentlyReading')}>
          Add to Currently Reading
        </button>
      </div>
    );
  }

  const lower = readingStatus.toLowerCase();

  return (
    <div className="reading-status">
      {lower === readingListStatus.read.toLowerCase() && (
        <p>
          You marked this book as <strong>Read</strong>.
        </p>
      )}
      {lower === readingListStatus.toRead.toLowerCase() && (
        <p>
          You added this book to <strong>Want to Read</strong>.
        </p>
      )}
      {lower === readingListStatus.currentlyReading.toLowerCase() && (
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
