import type { AuthState } from './types/authState.type';
import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

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
