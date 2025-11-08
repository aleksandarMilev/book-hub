import { jwtDecode } from 'jwt-decode';

import * as profileApi from '@/api/profile/profileApi';
import identityApi from '@/features/identity/api/api';
import type {
  DecodedToken,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from '@/features/identity/types/identity';
import { usePublicPost } from '@/shared/hooks/usePublicPost';
import { useAuth } from '@/shared/stores/auth/auth';
import type { User } from '@/shared/stores/auth/types/user';

export function useLogin() {
  const { changeAuthenticationState } = useAuth();

  return usePublicPost<LoginRequest, LoginResponse>(
    (data, signal) => identityApi.publicPost<LoginRequest, LoginResponse>(data, signal, 'login'),
    'Login failed.',
    async (result) => {
      const decoded = jwtDecode<DecodedToken>(result.token);
      const user: User = {
        userId: decoded.nameid,
        username: decoded.unique_name,
        email: decoded.email,
        token: result.token,
        isAdmin: Boolean(decoded.role),
        hasProfile: await profileApi.hasProfile(result.token),
      };

      changeAuthenticationState(user);
    },
  );
}

export function useRegister() {
  const { changeAuthenticationState } = useAuth();

  return usePublicPost<RegisterRequest, LoginResponse>(
    (data, signal) =>
      identityApi.publicPost<RegisterRequest, LoginResponse>(data, signal, 'register'),
    'Registration failed.',
    async (result) => {
      const decoded = jwtDecode<DecodedToken>(result.token);
      const user: User = {
        userId: decoded.nameid,
        username: decoded.unique_name,
        email: decoded.email,
        token: result.token,
        isAdmin: Boolean(decoded.role),
        hasProfile: await profileApi.hasProfile(result.token),
      };

      changeAuthenticationState(user);
    },
  );
}
