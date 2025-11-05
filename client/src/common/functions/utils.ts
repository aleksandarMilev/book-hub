import { format } from 'date-fns';

export const parseId = (idAsString?: string) => {
  const id = Number(idAsString);
  if (!Number.isInteger(id) || id <= 0) {
    throw new Error('Invalid id.');
  }

  return id;
};

export const formatIsoDate = (iso?: string | null, fallback = 'Unknown date') =>
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

export const calculateAge = (bornAtISO: string, endISO?: string) => {
  const start = new Date(bornAtISO);
  const end = endISO ? new Date(endISO) : new Date();
  const month = end.getMonth() - start.getMonth();

  let age = end.getFullYear() - start.getFullYear();

  const birthdayIsNotReached = month < 0 || (month === 0 && end.getDate() < start.getDate());
  if (birthdayIsNotReached) {
    age--;
  }

  return age;
};
