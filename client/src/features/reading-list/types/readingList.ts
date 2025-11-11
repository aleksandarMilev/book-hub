export type ReadingStatusUI = 'read' | 'to read' | 'currently reading';

export type ReadingStatusAPI = 'Read' | 'ToRead' | 'CurrentlyReading';

export const toApiStatus = (status: ReadingStatusUI): ReadingStatusAPI => {
  switch (status) {
    case 'read':
      return 'Read';
    case 'to read':
      return 'ToRead';
    case 'currently reading':
      return 'CurrentlyReading';
  }
};

export const toUiStatus = (status: ReadingStatusAPI): ReadingStatusUI => {
  switch (status) {
    case 'Read':
      return 'read';
    case 'ToRead':
      return 'to read';
    case 'CurrentlyReading':
      return 'currently reading';
  }
};
