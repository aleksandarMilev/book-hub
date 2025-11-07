import { create } from 'zustand';
import { createJSONStorage, persist } from 'zustand/middleware';

import type { AuthState } from './types/authState.type';

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      setUser: (user) => set({ user }),
      setHasProfile: (hasProfile) =>
        set((state) => (state.user ? { user: { ...state.user, hasProfile } } : state)),
      logout: () => set({ user: null }),
    }),
    {
      name: 'user',
      storage: createJSONStorage(() => localStorage),
    },
  ),
);

export const selectUser = (state: AuthState) => state.user;

export const selectIsAuthenticated = (state: AuthState) => Boolean(state.user?.username);

export const selectToken = (state: AuthState) => state.user?.token ?? '';
