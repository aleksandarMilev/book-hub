import { useCallback, useContext } from 'react';
import { jwtDecode } from 'jwt-decode';

import * as profileApi from '../../api/profile/profileApi';
import * as identityApi from '../../api/identity/identityApi';
import { UserContext } from '../../contexts/user/userContext';
import type { LoginResponse } from '../../api/identity/types/loginResponse';
import type { User } from '../../stores/auth/types/user.type';
import type { JwtPayload } from './types/jwtPayload';

export function useLogin() {
  const { changeAuthenticationState } = useContext(UserContext);

  const onLogin = useCallback(
    async (credentials: string, password: string, rememberMe: boolean): Promise<void> => {
      try {
        const result: LoginResponse = await identityApi.login(credentials, password, rememberMe);
        const decoded = jwtDecode<JwtPayload>(result.token);
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
        const message = error instanceof Error ? error.message : 'Login failed.';
        throw new Error(message);
      }
    },
    [changeAuthenticationState],
  );

  return onLogin;
}

export function useRegister() {
  const { changeAuthenticationState } = useContext(UserContext);

  const onRegister = useCallback(
    async (username: string, email: string, password: string): Promise<void> => {
      try {
        const result: LoginResponse = await identityApi.register(username, email, password);
        const decoded = jwtDecode<JwtPayload>(result.token);
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
        const message = error instanceof Error ? error.message : 'Registration failed.';
        throw new Error(message);
      }
    },
    [changeAuthenticationState],
  );

  return onRegister;
}
