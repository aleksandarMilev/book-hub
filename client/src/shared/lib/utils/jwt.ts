import { jwtDecode } from 'jwt-decode';

// True when the token can't be decoded, has no numeric `exp`, or `exp` is not in the future.
export const isTokenExpired = (token: string, nowMs: number = Date.now()): boolean => {
  try {
    const { exp } = jwtDecode(token);

    return typeof exp !== 'number' || exp * 1000 <= nowMs;
  } catch {
    return true;
  }
};
