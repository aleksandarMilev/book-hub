import type { User } from '@/shared/stores/auth/types/user';

export type AuthView = {
  userId: string;
  username: string;
  email: string;
  token: string;
  isAdmin: boolean;
  isAuthenticated: boolean;
  changeAuthenticationState: (user: User | null) => void;
  resetAuth: () => void;
  logout: () => void;
};


