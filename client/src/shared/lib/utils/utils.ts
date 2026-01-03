import { HttpStatusCode, isAxiosError } from 'axios';
import { format } from 'date-fns';

import { baseUrl } from '@/shared/lib/constants/api.js';

export const slugify = (value: string): string =>
  value
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/[^a-z0-9]+/g, '-')
    .replace(/^-+|-+$/g, '');

export const getImageUrl = (imagePath: string, resource: string) => {
  if (!imagePath) {
    return `${baseUrl}/images/${resource}/default.jpg`;
  }

  if (imagePath.startsWith('http://') || imagePath.startsWith('https://')) {
    return imagePath;
  }

  return `${baseUrl}${imagePath.startsWith('/') ? imagePath : `/${imagePath}`}`;
};

export const sleep = (ms = 2_000) => new Promise<void>((resolve) => setTimeout(resolve, ms));

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

export const IsError = (error: unknown): error is Error => {
  return error instanceof Error;
};

export const IsDomException = (error: unknown): error is DOMException => {
  return error instanceof DOMException;
};

export const IsAbortError = (error: unknown): error is DOMException | Error => {
  return (
    (IsError(error) && error.name === 'AbortError') ||
    (IsDomException(error) && error.name === 'AbortError')
  );
};

export const IsCanceledError = (error: unknown): error is Error => {
  return IsError(error) && error.name === 'CanceledError';
};

export const IsDomAbortError = (error: unknown): error is DOMException => {
  return IsDomException(error) && error.name === 'AbortError';
};

export const isNotFoundError = (error: unknown): error is Error => {
  return isAxiosError(error) && error.response?.status === HttpStatusCode.NotFound;
};
