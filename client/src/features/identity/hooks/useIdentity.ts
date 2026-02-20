import { jwtDecode } from 'jwt-decode';
import { useCallback } from 'react';
import { useTranslation } from 'react-i18next';

import * as identityApi from '@/features/identity/api/api';
import type {
  DecodedToken,
  LoginResponse,
  RegisterFormValues,
} from '@/features/identity/types/identity';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import type { User } from '@/shared/stores/auth/types/user';
import { useMessage } from '@/shared/stores/message/message';

export const useLogin = () => {
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();
  const { t } = useTranslation('identity');

  const onLogin = useCallback(
    async (credentials: string, password: string, rememberMe: boolean) => {
      try {
        const result: LoginResponse = await identityApi.login(credentials, password, rememberMe);
        const decoded = jwtDecode<DecodedToken>(result.token);
        const user = userFromDecodedToken(decoded, result.token);

        changeAuthenticationState(user);
        showMessage(t('messages.welcome', { username: user.username }), true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const errorMessage =
          IsError(error) && error.message ? error.message : t('messages.loginFailed');

        throw new Error(errorMessage);
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
        const user = userFromDecodedToken(decoded, result.token);

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

const userFromDecodedToken = (decoded: DecodedToken, token: string): User => ({
  userId: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
  username: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
  email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
  isAdmin: Boolean(decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']),
  token,
});
