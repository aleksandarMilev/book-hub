import type { User } from './user.type';

export interface AuthState {
  user: User | null;
  setUser: (u: User | null) => void;
  setHasProfile: (hasProfile: boolean) => void;
  logout: () => void;
}
