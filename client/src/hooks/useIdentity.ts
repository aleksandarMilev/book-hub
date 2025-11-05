import { useCallback, useContext } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';

import * as profileApi from '../api/profile/profileApi';
import * as identityApi from '../api/identity/identityApi';
import { UserContext } from '../contexts/user/userContext';
import { routes } from '../common/constants/api';
import type { User } from '../stores/auth/types/user.type';
import type { DecodedToken, LoginResponse } from '../api/identity/types/identity';

export function useLogin() {
  const navigate = useNavigate();
  const { changeAuthenticationState } = useContext(UserContext);

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
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') return;

        const message = error instanceof Error ? error.message : 'Login failed.';
        navigate(routes.badRequest, { state: { message } });
      }

      return () => controller.abort();
    },
    [changeAuthenticationState, navigate],
  );

  return onLogin;
}

export function useRegister() {
  const navigate = useNavigate();
  const { changeAuthenticationState } = useContext(UserContext);

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
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') return;

        const message = error instanceof Error ? error.message : 'Registration failed.';
        navigate(routes.badRequest, { state: { message } });
      }

      return () => controller.abort();
    },
    [changeAuthenticationState, navigate],
  );

  return onRegister;
}
