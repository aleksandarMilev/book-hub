import {
  getAuthConfig,
  getAuthConfigForFile,
  getPublicConfig,
  processError,
} from '@/shared/api/http.js';

describe('http config helpers', () => {
  describe('getAuthConfig()', () => {
    it('should build config without signal', () => {
      const token = 'test-token';

      const config = getAuthConfig(token);

      expect(config.headers).toBeDefined();
      expect(config.headers?.Authorization).toBe(`Bearer ${token}`);
      expect(config.headers?.['Content-Type']).toBe('application/json');
      expect(config.signal).toBeUndefined();
    });

    it('should include signal when provided', () => {
      const token = 'test-token';
      const controller = new AbortController();

      const config = getAuthConfig(token, controller.signal);

      expect(config.headers?.Authorization).toBe(`Bearer ${token}`);
      expect(config.headers?.['Content-Type']).toBe('application/json');
      expect(config.signal).toBe(controller.signal);
    });
  });

  describe('getAuthConfigForFile()', () => {
    it('should override content-type and keep auth headers and signal', () => {
      const token = 'file-token';
      const controller = new AbortController();

      const config = getAuthConfigForFile(token, controller.signal);

      const headers = config.headers as Record<string, string>;

      expect(headers.Authorization).toBe(`Bearer ${token}`);
      expect(headers['Content-Type']).toBe('multipart/form-data');
      expect(config.signal).toBe(controller.signal);
    });
  });

  describe('getPublicConfig()', () => {
    it('should build public config without signal', () => {
      const config = getPublicConfig();

      expect(config.headers).toBeDefined();
      expect(config.headers?.['Content-Type']).toBe('application/json');
      expect(config.signal).toBeUndefined();
    });

    it('should include signal when provided', () => {
      const controller = new AbortController();
      const config = getPublicConfig(controller.signal);

      expect(config.headers?.['Content-Type']).toBe('application/json');
      expect(config.signal).toBe(controller.signal);
    });
  });
});

describe('processError()', () => {
  it('should rethrow the original error if it is a canceled request', () => {
    const canceledError = new Error('Request was canceled');
    canceledError.name = 'CanceledError';

    try {
      processError(canceledError, 'Some message');
    } catch (error) {
      expect(error).toBe(canceledError);
    }
  });

  it('should throw a new error with the provided message for non-canceled errors', () => {
    const originalError = new Error('Something else went wrong');
    expect(() => processError(originalError, 'Custom fallback message')).toThrowError(
      'Custom fallback message',
    );
  });
});
