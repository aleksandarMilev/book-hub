import { router } from '@/app/routes';
import { onSessionExpired } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { isTokenExpired } from '@/shared/lib/utils/jwt';
import { getAuthToken, resetAuth } from '@/shared/stores/auth/auth';

// Runs once before the first render. The persisted auth store hydrates synchronously from
// localStorage, so the token is already available here.
export const initSession = () => {
  const token = getAuthToken();
  if (token !== null && isTokenExpired(token)) {
    resetAuth();
  }

  // `router` is the data router passed to <RouterProvider>, so it can navigate from outside React.
  onSessionExpired(() => {
    resetAuth();

    if (router.state.location.pathname !== routes.login) {
      void router.navigate(routes.login, { replace: true });
    }
  });
};
