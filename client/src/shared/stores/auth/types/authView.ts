import type { User } from '@/shared/stores/auth/types/user.js';

export type AuthView = {
  userId: string;
  username: string;
  email: string;
  token: string;
  isAdmin: boolean;
  isAuthenticated: boolean;
  hasProfile: boolean;
  changeAuthenticationState: (user: User | null) => void;
  changeHasProfileState: (hasProfile: boolean) => void;
  logout: () => void;
};
