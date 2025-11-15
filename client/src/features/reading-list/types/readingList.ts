export type ReadingStatusUI = 'read' | 'to read' | 'currently reading' | null;

export type ReadingStatusAPI = 'Read' | 'ToRead' | 'CurrentlyReading' | null;

export const toApiStatus = (status: string | null | undefined): ReadingStatusAPI => {
  switch (status) {
    case 'read':
      return 'Read';
    case 'to read':
      return 'ToRead';
    case 'currently reading':
      return 'CurrentlyReading';
    default:
      return null;
  }
};

export const toUiStatus = (status: string | null | undefined): ReadingStatusUI => {
  switch (status) {
    case 'Read':
      return 'read';
    case 'ToRead':
      return 'to read';
    case 'CurrentlyReading':
      return 'currently reading';
    default:
      return null;
  }
};
