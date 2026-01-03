import { createJSONStorage, persist } from 'zustand/middleware';
import { shallow } from 'zustand/shallow';
import { createWithEqualityFn } from 'zustand/traditional';
import type { StateCreator } from 'zustand/vanilla';

import type { AuthState } from './types/authState.js';
import type { AuthView } from './types/authView.js';

const creator: StateCreator<AuthState> = (set) => ({
  user: null,
  setUser: (user) => set({ user }),
  logout: () => set({ user: null }),
  resetAuth: () => {
    set({ user: null });
    localStorage.removeItem('auth');
  },
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
    changeAuthenticationState: state.setUser,
    logout: state.logout,
    resetAuth: state.resetAuth,
  }));
