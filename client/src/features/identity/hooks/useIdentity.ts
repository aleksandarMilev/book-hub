import { jwtDecode } from 'jwt-decode';
import { useCallback } from 'react';
import { useTranslation } from 'react-i18next';

import * as identityApi from '@/features/identity/api/api.js';
import type {
  DecodedToken,
  LoginResponse,
  RegisterFormValues,
} from '@/features/identity/types/identity.js';
import { IsCanceledError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import type { User } from '@/shared/stores/auth/types/user.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useLogin = () => {
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();
  const { t } = useTranslation('identity');

  const onLogin = useCallback(
    async (credentials: string, password: string, rememberMe: boolean) => {
      try {
        const result: LoginResponse = await identityApi.login(credentials, password, rememberMe);
        const decoded = jwtDecode<DecodedToken>(result.token);

        const user: User = {
          userId: decoded.nameid,
          username: decoded.unique_name,
          email: decoded.email,
          token: result.token,
          isAdmin: Boolean(decoded.role),
        };

        changeAuthenticationState(user);
        showMessage(t('messages.welcome', { username: user.username }), true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = t('messages.loginFailed');
        throw new Error(message);
      }
    },
    [changeAuthenticationState, showMessage, t],
  );

  return onLogin;
};

export const useRegister = () => {
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();
  const { t } = useTranslation('identity');

  const onRegister = useCallback(
    async (requestModel: RegisterFormValues) => {
      try {
        const result: LoginResponse = await identityApi.register(requestModel);
        if (!result) {
          return;
        }

        const decoded = jwtDecode<DecodedToken>(result.token);
        const user: User = {
          userId: decoded.nameid,
          username: decoded.unique_name,
          email: decoded.email,
          token: result.token,
          isAdmin: Boolean(decoded.role),
        };

        changeAuthenticationState(user);
        showMessage(t('messages.welcome', { username: user.username }), true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        throw error;
      }
    },
    [changeAuthenticationState, showMessage, t],
  );

  return onRegister;
};
