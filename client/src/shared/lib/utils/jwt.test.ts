import { isTokenExpired } from '@/shared/lib/utils/jwt';

const NOW_MS = Date.UTC(2026, 9, 2, 12, 0, 0);
const NOW_SECONDS = NOW_MS / 1000;

const base64Url = (value: object) =>
  btoa(JSON.stringify(value)).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');

const createToken = (payload: object) =>
  `${base64Url({ alg: 'HS256', typ: 'JWT' })}.${base64Url(payload)}.signature`;

describe('isTokenExpired', () => {
  it('returns false for a token that expires in the future', () => {
    const token = createToken({ sub: 'user-id', exp: NOW_SECONDS + 60 });

    expect(isTokenExpired(token, NOW_MS)).toBe(false);
  });

  it('returns true for a token that has expired', () => {
    const token = createToken({ sub: 'user-id', exp: NOW_SECONDS - 60 });

    expect(isTokenExpired(token, NOW_MS)).toBe(true);
  });

  it('returns true for a token that expires exactly now', () => {
    const token = createToken({ exp: NOW_SECONDS });

    expect(isTokenExpired(token, NOW_MS)).toBe(true);
  });

  it('returns true for a token without a numeric exp claim', () => {
    expect(isTokenExpired(createToken({ sub: 'user-id' }), NOW_MS)).toBe(true);
    expect(isTokenExpired(createToken({ exp: String(NOW_SECONDS + 60) }), NOW_MS)).toBe(true);
  });

  it.each(['', 'not-a-jwt', 'a.b.c', 'header.%%%.signature'])(
    'returns true for the malformed token %j',
    (token) => {
      expect(isTokenExpired(token, NOW_MS)).toBe(true);
    },
  );
});
