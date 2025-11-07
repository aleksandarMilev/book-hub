import { createContext, useCallback, useMemo, type PropsWithChildren } from 'react';

import type { UserContextValue } from './types/userContextValue.type';

import { selectIsAuthenticated, selectUser, useAuthStore } from '../../stores/auth/auth';
import type { User } from '../../stores/auth/types/user.type';

export const UserContext = createContext<UserContextValue>({
  userId: '',
  username: '',
  email: '',
  token: '',
  isAdmin: false,
  isAuthenticated: false,
  hasProfile: false,
  changeAuthenticationState: () => {},
  changeHasProfileState: () => {},
  logout: () => {},
});

export function UserContextProvider({ children }: PropsWithChildren) {
  const user = useAuthStore(selectUser);
  const isAuthenticated = useAuthStore(selectIsAuthenticated);

  const changeAuthenticationState = useAuthStore((s) => s.setUser);
  const changeHasProfileState = useAuthStore((s) => s.setHasProfile);
  const logout = useAuthStore((s) => s.logout);

  const setAuth = useCallback(
    (user: User | null) => changeAuthenticationState(user),
    [changeAuthenticationState],
  );
  const setHasProfile = useCallback(
    (hasProfile: boolean) => changeHasProfileState(hasProfile),
    [changeHasProfileState],
  );

  const value: UserContextValue = useMemo(
    () => ({
      userId: user?.userId ?? '',
      username: user?.username ?? '',
      email: user?.email ?? '',
      token: user?.token ?? '',
      isAdmin: user?.isAdmin ?? false,
      isAuthenticated,
      hasProfile: user?.hasProfile ?? false,
      changeAuthenticationState: setAuth,
      changeHasProfileState: setHasProfile,
      logout,
    }),
    [user, isAuthenticated, setAuth, setHasProfile, logout],
  );

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
}
