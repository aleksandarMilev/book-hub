import type { User } from '@/shared/stores/auth/types/user.js';

export interface AuthState {
  user: User | null;
  setUser: (user: User | null) => void;
  logout: () => void;
}
