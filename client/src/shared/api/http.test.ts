import {
  type AxiosAdapter,
  AxiosError,
  AxiosHeaders,
  type AxiosInstance,
  type AxiosResponse,
  CanceledError,
  type InternalAxiosRequestConfig,
} from 'axios';

import {
  getAuthConfig,
  getProblemMessage,
  getPublicConfig,
  http,
  httpAdmin,
  onSessionExpired,
  processError,
} from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';

const okResponse = (config: InternalAxiosRequestConfig): AxiosResponse => ({
  data: null,
  status: 200,
  statusText: 'OK',
  headers: {},
  config,
});

// An adapter that records the request instead of sending it.
const recordingAdapter = () => {
  const requests: InternalAxiosRequestConfig[] = [];
  const adapter: AxiosAdapter = (config) => {
    requests.push(config);

    return Promise.resolve(okResponse(config));
  };

  return { adapter, requests };
};

// An adapter that fails every request with the given status, as axios's own adapters do.
const failingAdapter =
  (status: number): AxiosAdapter =>
  (config) =>
    Promise.reject(
      new AxiosError(
        `Request failed with status code ${status}`,
        AxiosError.ERR_BAD_REQUEST,
        config,
        null,
        { ...okResponse(config), status, statusText: String(status) },
      ),
    );

describe('getAuthConfig', () => {
  it('sets the bearer token and does not force a Content-Type', () => {
    expect(getAuthConfig('token-value').headers).toEqual({ Authorization: 'Bearer token-value' });
  });

  it('passes the abort signal through', () => {
    const controller = new AbortController();

    expect(getAuthConfig('token-value', controller.signal).signal).toBe(controller.signal);
  });

  it('lets axios send FormData as multipart instead of serializing it to JSON', async () => {
    const { adapter, requests } = recordingAdapter();
    const formData = new FormData();
    formData.append('title', 'A book');
    formData.append('image', new File(['image-bytes'], 'cover.png', { type: 'image/png' }));

    await http.post('/books', formData, { ...getAuthConfig('token-value'), adapter });

    const sent = requests[0];
    expect(sent?.data).toBe(formData);
    expect(String(sent?.headers.getContentType() ?? '')).not.toContain('application/json');
  });

  it('still sends plain objects as JSON', async () => {
    const { adapter, requests } = recordingAdapter();

    await http.post('/reviews', { content: 'Great' }, { ...getAuthConfig('token-value'), adapter });

    const sent = requests[0];
    expect(sent?.data).toBe(JSON.stringify({ content: 'Great' }));
    expect(String(sent?.headers.getContentType())).toContain('application/json');
  });
});

describe('getPublicConfig', () => {
  it('does not force a Content-Type or an Authorization header', () => {
    const config = getPublicConfig();

    expect(config.headers).toBeUndefined();
  });
});

describe('onSessionExpired', () => {
  let removeInterceptors: () => void = () => {};
  const onExpired = vi.fn();

  beforeEach(() => {
    onExpired.mockReset();
    removeInterceptors = onSessionExpired(onExpired);
  });

  afterEach(() => {
    removeInterceptors();
  });

  const request = (instance: AxiosInstance, url: string, status: number, token?: string) =>
    instance.get(url, {
      ...(token ? getAuthConfig(token) : getPublicConfig()),
      adapter: failingAdapter(status),
    });

  it.each([
    ['http', http],
    ['httpAdmin', httpAdmin],
  ])(
    'calls the handler on a 401 for an authenticated %s request and rethrows',
    async (_, instance) => {
      await expect(request(instance, '/books/1', 401, 'expired-token')).rejects.toBeInstanceOf(
        AxiosError,
      );

      expect(onExpired).toHaveBeenCalledTimes(1);
    },
  );

  it('ignores a 401 for a request sent without a token', async () => {
    await expect(request(http, '/books/1', 401)).rejects.toBeInstanceOf(AxiosError);

    expect(onExpired).not.toHaveBeenCalled();
  });

  it('ignores a 401 for a request with an empty token', async () => {
    await expect(request(http, '/books/1', 401, ' ')).rejects.toBeInstanceOf(AxiosError);

    expect(onExpired).not.toHaveBeenCalled();
  });

  it.each([routes.login, routes.register, routes.forgotPassword, routes.resetPassword])(
    'ignores a 401 from the identity endpoint %s',
    async (url) => {
      await expect(request(http, url, 401, 'token-value')).rejects.toBeInstanceOf(AxiosError);

      expect(onExpired).not.toHaveBeenCalled();
    },
  );

  it.each([400, 403, 404, 500])('ignores a %i response', async (status) => {
    await expect(request(http, '/books/1', status, 'token-value')).rejects.toBeInstanceOf(
      AxiosError,
    );

    expect(onExpired).not.toHaveBeenCalled();
  });

  it('stops calling the handler once the interceptors are removed', async () => {
    removeInterceptors();

    await expect(request(http, '/books/1', 401, 'expired-token')).rejects.toBeInstanceOf(
      AxiosError,
    );

    expect(onExpired).not.toHaveBeenCalled();
  });
});

// An axios error as the response interceptor sees it: the server answered with `status` and `data`.
const responseError = (status: number, data: unknown) => {
  const config = { headers: new AxiosHeaders() } as InternalAxiosRequestConfig;

  return new AxiosError(`Request failed with status code ${status}`, undefined, config, null, {
    ...okResponse(config),
    status,
    data,
  });
};

const messageOf = (error: unknown) => {
  try {
    processError(error, 'Fallback message');
  } catch (thrown) {
    return thrown instanceof Error ? thrown.message : String(thrown);
  }

  throw new Error('processError did not throw');
};

describe('processError', () => {
  it('uses the ProblemDetails detail', () => {
    const problem = { title: 'Not Found', status: 404, detail: 'The book was not found.' };

    expect(messageOf(responseError(404, problem))).toBe('The book was not found.');
  });

  it('uses the first field message of a validation problem', () => {
    const problem = {
      title: 'One or more validation errors occurred.',
      status: 400,
      errors: { Title: ['The Title field is required.'], Pages: ['Too many pages.'] },
    };

    expect(messageOf(responseError(400, problem))).toBe('The Title field is required.');
  });

  it.each([403, 409])('uses the detail of a %i problem', (status) => {
    expect(messageOf(responseError(status, { status, detail: 'Specific message' }))).toBe(
      'Specific message',
    );
  });

  it('falls back when the problem has only a generic title', () => {
    expect(messageOf(responseError(404, { title: 'Not Found', status: 404 }))).toBe(
      'Fallback message',
    );
  });

  it('falls back for a server error, whose detail is generic', () => {
    const problem = { status: 500, detail: 'An unexpected error occurred.', traceId: '00-abc' };

    expect(messageOf(responseError(500, problem))).toBe('Fallback message');
  });

  it.each([null, '', 'plain text body', { detail: '   ' }])(
    'falls back for the body %j',
    (data) => {
      expect(messageOf(responseError(400, data))).toBe('Fallback message');
    },
  );

  it('falls back for a network error without a response', () => {
    expect(messageOf(new AxiosError('Network Error', AxiosError.ERR_NETWORK))).toBe(
      'Fallback message',
    );
  });

  it('rethrows a cancellation unchanged', () => {
    const canceled = new CanceledError();

    expect(() => processError(canceled, 'Fallback message')).toThrow(canceled);
  });
});

describe('getProblemMessage', () => {
  it('ignores non-string field messages', () => {
    expect(getProblemMessage({ errors: { Title: [42, ''], Pages: ['Too many pages.'] } })).toBe(
      'Too many pages.',
    );
  });
});
