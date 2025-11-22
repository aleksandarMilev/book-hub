import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { type Mock, vi } from 'vitest';

import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

import AdminRoute from './AdminRoute.js';

vi.mock('@/shared/stores/auth/auth.js', () => ({
  useAuth: vi.fn(),
}));

const mockUseAuth = useAuth as unknown as Mock;

const setGuest = () => {
  mockUseAuth.mockReturnValue({
    isAuthenticated: false,
    isAdmin: false,
  });
};

const setUser = () => {
  mockUseAuth.mockReturnValue({
    isAuthenticated: true,
    isAdmin: false,
  });
};

const setAdmin = () => {
  mockUseAuth.mockReturnValue({
    isAuthenticated: true,
    isAdmin: true,
  });
};

const renderAdminRoute = () => {
  return render(
    <MemoryRouter initialEntries={['/admin-only']}>
      <Routes>
        <Route path="/admin-only" element={<AdminRoute element={<div>Protected content</div>} />} />
        <Route path={routes.login} element={<div>Login Page</div>} />
        <Route path={routes.accessDenied} element={<div>Access Denied</div>} />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Admin Route Guard', () => {
  it('should redirect to login if user is guest', () => {
    setGuest();
    renderAdminRoute();

    expect(screen.getByText(/login page/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should redirect to access denied if user is logged but not admin', () => {
    setUser();
    renderAdminRoute();

    expect(screen.getByText(/access denied/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should return the protected element if user is admin', () => {
    setAdmin();
    renderAdminRoute();

    expect(screen.getByText(/protected content/i)).toBeInTheDocument();
  });
});
