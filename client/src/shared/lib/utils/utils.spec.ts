import { HttpStatusCode } from 'axios';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

import { baseUrl } from '@/shared/lib/constants/api.js';

import {
  calculateAge,
  formatIsoDate,
  getImageUrl,
  IsAbortError,
  IsCanceledError,
  IsDomAbortError,
  IsDomException,
  IsError,
  isNotFoundError,
  sleep,
  toIntId,
} from './utils.js';

describe('Util Functions', () => {
  describe('getImageUrl()', () => {
    it('should return default image when imagePath is empty', () => {
      const url = getImageUrl('', 'books');
      expect(url).toBe(`${baseUrl}/images/books/default.avif`);
    });

    it('should return the same URL when imagePath is an absolute http url', () => {
      const absolute = 'http://example.com/image.jpg';
      const url = getImageUrl(absolute, 'books');

      expect(url).toBe(absolute);
    });

    it('should return the same URL when imagePath is an absolute https url', () => {
      const absolute = 'https://cdn.example.com/image.png';
      const url = getImageUrl(absolute, 'books');

      expect(url).toBe(absolute);
    });

    it('should prefix baseUrl when imagePath starts with a slash', () => {
      const url = getImageUrl('/uploads/image.jpg', 'books');
      expect(url).toBe(`${baseUrl}/uploads/image.jpg`);
    });

    it('should prefix baseUrl and inserts a slash when imagePath has no leading slash', () => {
      const url = getImageUrl('uploads/image.jpg', 'books');
      expect(url).toBe(`${baseUrl}/uploads/image.jpg`);
    });
  });

  describe('sleep()', () => {
    beforeEach(() => {
      vi.useFakeTimers();
    });

    afterEach(() => {
      vi.useRealTimers();
    });

    it('should resolve after the given delay', async () => {
      let finished = false;

      const promise = sleep(1_000).then(() => {
        finished = true;
      });

      expect(finished).toBe(false);

      vi.advanceTimersByTime(999);

      await Promise.resolve();
      expect(finished).toBe(false);

      vi.advanceTimersByTime(1);
      await promise;

      expect(finished).toBe(true);
    });
  });

  describe('toIntId()', () => {
    it('should return IntId for positive integer numbers', () => {
      expect(toIntId(5)).toBe(5);
    });

    it('should return IntId for positive integer strings', () => {
      expect(toIntId('10')).toBe(10);
    });

    it('should return null for non-integer numbers', () => {
      expect(toIntId(3.14)).toBeNull();
    });

    it('should return null for non-numeric strings', () => {
      expect(toIntId('abc')).toBeNull();
    });

    it('should return null for zero or negative values', () => {
      expect(toIntId(0)).toBeNull();
      expect(toIntId(-1)).toBeNull();
      expect(toIntId('-5')).toBeNull();
    });

    it('should return null for non-number types', () => {
      expect(toIntId({})).toBeNull();
      expect(toIntId(null)).toBeNull();
      expect(toIntId(undefined)).toBeNull();
    });
  });

  describe('formatIsoDate()', () => {
    it('should return formatted date for a valid ISO string', () => {
      const iso = '2023-05-15T00:00:00';
      const formatted = formatIsoDate(iso);

      expect(formatted).toBe('15 May 2023');
    });

    it('should return default fallback when iso is null/undefined', () => {
      expect(formatIsoDate(null)).toBe('Unknown date');
      expect(formatIsoDate(undefined)).toBe('Unknown date');
    });

    it('should return custom fallback when provided and iso is missing', () => {
      expect(formatIsoDate(null, 'N/A')).toBe('N/A');
    });
  });

  describe('calculateAge()', () => {
    it('should calculate age when birthday has already passed this year', () => {
      const bornAt = '2000-01-01';
      const end = '2020-06-01';

      const age = calculateAge(bornAt, end);
      expect(age).toBe(20);
    });

    it('should subtract one year when birthday has not yet been reached this year', () => {
      const bornAt = '2000-10-10';
      const end = '2020-10-09';

      const age = calculateAge(bornAt, end);
      expect(age).toBe(19);
    });

    it('should return 0 for same day birth and end date', () => {
      const bornAt = '2020-05-05';
      const end = '2020-05-05';

      const age = calculateAge(bornAt, end);
      expect(age).toBe(0);
    });
  });

  describe('Error Type Guards', () => {
    it('IsError() should return true only for Error instances', () => {
      const error = new Error('boom');
      const domException = new DOMException('dom');
      const notErrorObject = { message: 'not error' };

      expect(IsError(error)).toBe(true);
      expect(IsError(domException)).toBe(false);
      expect(IsError(notErrorObject)).toBe(false);
    });

    it('IsDomException() should return true only for DOMException instances', () => {
      const error = new Error('boom');
      const domException = new DOMException('dom');
      const domExceptionObject = { name: 'DOMException' };

      expect(IsDomException(domException)).toBe(true);
      expect(IsDomException(error)).toBe(false);
      expect(IsDomException(domExceptionObject)).toBe(false);
    });

    it('IsAbortError() should detect AbortError from Error or DOMException', () => {
      const abortError = new Error('aborted');
      abortError.name = 'AbortError';

      const domAbort = new DOMException('dom aborted', 'AbortError');

      const normalError = new Error('normal');
      const otherDom = new DOMException('dom', 'SomethingElse');

      expect(IsAbortError(abortError)).toBe(true);
      expect(IsAbortError(domAbort)).toBe(true);
      expect(IsAbortError(normalError)).toBe(false);
      expect(IsAbortError(otherDom)).toBe(false);
    });

    it('IsCanceledError() should detect CanceledError by name', () => {
      const canceled = new Error('canceled');
      canceled.name = 'CanceledError';

      const other = new Error('other');

      expect(IsCanceledError(canceled)).toBe(true);
      expect(IsCanceledError(other)).toBe(false);
    });

    it('IsDomAbortError() should detect DOMException AbortError only', () => {
      const domAbort = new DOMException('dom aborted', 'AbortError');
      const domOther = new DOMException('dom', 'SomethingElse');
      const normalError = new Error('err');

      expect(IsDomAbortError(domAbort)).toBe(true);
      expect(IsDomAbortError(domOther)).toBe(false);
      expect(IsDomAbortError(normalError)).toBe(false);
    });
  });

  describe('isNotFoundError()', () => {
    it('should return true for axios-style errors with 404 status', () => {
      const axiosLikeError = {
        isAxiosError: true,
        response: { status: HttpStatusCode.NotFound },
      };

      expect(isNotFoundError(axiosLikeError)).toBe(true);
    });

    it('should return for axios-style errors with non-404 status', () => {
      const axiosLikeError = {
        isAxiosError: true,
        response: { status: HttpStatusCode.BadRequest },
      };

      expect(isNotFoundError(axiosLikeError)).toBe(false);
    });

    it('should return false for non-axios errors', () => {
      const plainError = new Error('something went wrong');
      const randomObject = { response: { status: HttpStatusCode.NotFound } };

      expect(isNotFoundError(plainError)).toBe(false);
      expect(isNotFoundError(randomObject)).toBe(false);
    });
  });
});
