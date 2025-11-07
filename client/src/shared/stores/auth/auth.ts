import { createJSONStorage, persist } from 'zustand/middleware';
import { shallow } from 'zustand/shallow';
import { createWithEqualityFn } from 'zustand/traditional';
import type { StateCreator } from 'zustand/vanilla';

import type { AuthState } from './types/authState';
import type { AuthView } from './types/authView';

const creator: StateCreator<AuthState> = (set) => ({
  user: null,
  setUser: (user) => set({ user }),
  setHasProfile: (hasProfile) =>
    set((state) => (state.user ? { user: { ...state.user, hasProfile } } : state)),
  logout: () => set({ user: null }),
});

const useAuthStore = createWithEqualityFn<AuthState>()(
  persist(creator, {
    name: 'auth',
    storage: createJSONStorage(() => localStorage),
  }),
  shallow,
);

export const useAuth = (): AuthView =>
  useAuthStore((state) => ({
    userId: state.user?.userId ?? '',
    username: state.user?.username ?? '',
    email: state.user?.email ?? '',
    token: state.user?.token ?? '',
    isAdmin: state.user?.isAdmin ?? false,
    isAuthenticated: Boolean(state.user?.username),
    hasProfile: state.user?.hasProfile ?? false,
    changeAuthenticationState: state.setUser,
    changeHasProfileState: state.setHasProfile,
    logout: state.logout,
  }));
