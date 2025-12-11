export type ReadingStatusUI = 'read' | 'to read' | 'currently reading' | null;

// server enum: Read = 0, ToRead = 1, CurrentlyReading = 2
export type ReadingStatusAPI = 0 | 1 | 2;

export const toApiStatus = (status: ReadingStatusUI): ReadingStatusAPI | null => {
  switch (status) {
    case 'read':
      return 0;
    case 'to read':
      return 1;
    case 'currently reading':
      return 2;
    default:
      return null;
  }
};

export const toUiStatus = (status: number | null | undefined): ReadingStatusUI => {
  switch (status) {
    case 0:
      return 'read';
    case 1:
      return 'to read';
    case 2:
      return 'currently reading';
    default:
      return null;
  }
};
