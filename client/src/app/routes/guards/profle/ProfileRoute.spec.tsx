import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { type Mock, vi } from 'vitest';

import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

import ProfileRoute from './ProfileRoute.js';

vi.mock('@/shared/stores/auth/auth.js', () => ({
  useAuth: vi.fn(),
}));

const mockUseAuth = useAuth as unknown as Mock;

const setGuest = () => {
  mockUseAuth.mockReturnValue({ isAuthenticated: false, hasProfile: false, isAdmin: false });
};

const setUserWithoutProfile = () => {
  mockUseAuth.mockReturnValue({ isAuthenticated: true, hasProfile: false, isAdmin: false });
};

const setUserWithProfile = () => {
  mockUseAuth.mockReturnValue({ isAuthenticated: true, hasProfile: true, isAdmin: false });
};

const setAdmin = () => {
  mockUseAuth.mockReturnValue({ isAuthenticated: true, hasProfile: false, isAdmin: true });
};

const renderProfileRoute = () => {
  return render(
    <MemoryRouter initialEntries={['/profile-only']}>
      <Routes>
        <Route
          path="/profile-only"
          element={<ProfileRoute element={<div>Protected content</div>} />}
        />
        <Route path={routes.login} element={<div>Login Page</div>} />
        <Route
          path={routes.profile}
          element={<div>I have not a profile and I am not an admin</div>}
        />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Profile Route Guard', () => {
  it('should redirect to login if user is guest', () => {
    setGuest();
    renderProfileRoute();

    expect(screen.getByText(/login page/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should redirect to the profile page if user has not profile and is not an admin', () => {
    setUserWithoutProfile();
    renderProfileRoute();

    expect(screen.getByText(/i have not a profile and i am not an admin/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should return the protected element if user has not profile but is admin', () => {
    setAdmin();
    renderProfileRoute();

    expect(screen.getByText(/protected content/i)).toBeInTheDocument();
    expect(
      screen.queryByText(/i have not a profile and i am not an admin/i),
    ).not.toBeInTheDocument();
  });

  it('should return the protected element if user is logged and has profile', () => {
    setUserWithProfile();
    renderProfileRoute();

    expect(screen.getByText(/protected content/i)).toBeInTheDocument();
  });
});
