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
