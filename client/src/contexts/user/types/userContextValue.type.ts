import type { User } from '../../../stores/auth/types/user.type';

export interface UserContextValue {
  userId: string;
  username: string;
  email: string;
  token: string;
  isAdmin: boolean;
  isAuthenticated: boolean;
  hasProfile: boolean;
  changeAuthenticationState: (state: User | null) => void;
  changeHasProfileState: (hasProfile: boolean) => void;
  logout: () => void;
}
