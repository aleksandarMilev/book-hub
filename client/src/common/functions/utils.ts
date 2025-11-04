import { format } from 'date-fns';

export const parseId = (idAsString?: string) => {
  const id = Number(idAsString);
  if (!Number.isFinite(id)) {
    throw new Error('Invalid id.');
  }

  return id;
};

export const formatIsoDate = (iso?: string, fallback = 'Unknown date') =>
  iso ? format(new Date(iso), 'dd MMM yyyy') : fallback;

export const utcToLocal = (utcDateString: string) => {
  const isoDateString = utcDateString.replace(' ', 'T') + 'Z';
  const utcDate = new Date(isoDateString);
  const options: Intl.DateTimeFormatOptions = {
    weekday: 'short',
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
  };

  return utcDate.toLocaleString('en-GB', options);
};
