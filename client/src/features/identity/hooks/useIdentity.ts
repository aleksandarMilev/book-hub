import { jwtDecode } from 'jwt-decode';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as identityApi from '@/features/identity/api/api.js';
import type { DecodedToken, LoginResponse } from '@/features/identity/types/identity.js';
import * as profileApi from '@/features/profile/api/api.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import type { User } from '@/shared/stores/auth/types/user.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useLogin = () => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();

  const onLogin = useCallback(
    async (credentials: string, password: string, rememberMe: boolean) => {
      const controller = new AbortController();

      try {
        const result: LoginResponse = await identityApi.login(
          credentials,
          password,
          rememberMe,
          controller.signal,
        );

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
        showMessage(`Welcome, ${user.username}!`, true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Login failed!';
        navigate(routes.badRequest, { state: { message } });
      }

      return () => controller.abort();
    },
    [changeAuthenticationState, navigate, showMessage],
  );

  return onLogin;
};

export const useRegister = () => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();

  const onRegister = useCallback(
    async (username: string, email: string, password: string) => {
      const controller = new AbortController();

      try {
        const result: LoginResponse = await identityApi.register(
          username,
          email,
          password,
          controller.signal,
        );

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
        showMessage(`Welcome, ${user.username}!`, true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Registration failed!';
        navigate(routes.badRequest, { state: { message } });
      }

      return () => controller.abort();
    },
    [changeAuthenticationState, navigate, showMessage],
  );

  return onRegister;
};
