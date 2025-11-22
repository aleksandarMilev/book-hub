import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { type Mock, vi } from 'vitest';

import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

import AuthenticatedRoute from './AuthenticatedRoute.js';

vi.mock('@/shared/stores/auth/auth.js', () => ({
  useAuth: vi.fn(),
}));

const mockUseAuth = useAuth as unknown as Mock;

const setGuest = () => {
  mockUseAuth.mockReturnValue({
    isAuthenticated: false,
  });
};

const setUser = () => {
  mockUseAuth.mockReturnValue({
    isAuthenticated: true,
  });
};

const renderAuthenticatedRoute = () => {
  return render(
    <MemoryRouter initialEntries={['/auth-only']}>
      <Routes>
        <Route
          path="/auth-only"
          element={<AuthenticatedRoute element={<div>Protected content</div>} />}
        />
        <Route path={routes.login} element={<div>Login Page</div>} />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Authenticated Route Guard', () => {
  it('should redirect to login if user is guest', () => {
    setGuest();
    renderAuthenticatedRoute();

    expect(screen.getByText(/login page/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should return the protected element if user is logged', () => {
    setUser();
    renderAuthenticatedRoute();

    expect(screen.getByText(/protected content/i)).toBeInTheDocument();
  });
});
