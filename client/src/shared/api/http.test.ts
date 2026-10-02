import {
  type AxiosAdapter,
  AxiosError,
  type AxiosInstance,
  type AxiosResponse,
  type InternalAxiosRequestConfig,
} from 'axios';

import {
  getAuthConfig,
  getPublicConfig,
  http,
  httpAdmin,
  onSessionExpired,
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
