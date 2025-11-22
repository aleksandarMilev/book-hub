import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { type Mock, vi } from 'vitest';

import { useHasAccess } from '@/app/routes/guards/chat/hooks/useHasAccess.js';
import { routes } from '@/shared/lib/constants/api.js';

import ChatRoute from './ChatRoute.js';

vi.mock('@/app/routes/guards/chat/hooks/useHasAccess.js', () => ({
  useHasAccess: vi.fn(),
}));

const mockUseHasAccess = useHasAccess as unknown as Mock;

const setGuest = () =>
  mockUseHasAccess.mockReturnValue({
    isAuthenticated: false,
    hasAccess: false,
    isLoading: false,
  });

const setUserWithoutAccess = () =>
  mockUseHasAccess.mockReturnValue({
    isAuthenticated: true,
    hasAccess: false,
    isLoading: false,
  });

const setUserWithAccess = () =>
  mockUseHasAccess.mockReturnValue({
    isAuthenticated: true,
    hasAccess: true,
    isLoading: false,
  });

const setLoadingUser = () =>
  mockUseHasAccess.mockReturnValue({
    isAuthenticated: true,
    hasAccess: false,
    isLoading: true,
  });

const renderChatRoute = () => {
  return render(
    <MemoryRouter initialEntries={['/chat-only']}>
      <Routes>
        <Route path="/chat-only" element={<ChatRoute element={<div>Protected content</div>} />} />
        <Route path={routes.login} element={<div>Login Page</div>} />
        <Route path={routes.home} element={<div>Home Page</div>} />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Chat Route Guard', () => {
  it('should redirect to login if user is guest', () => {
    setGuest();
    renderChatRoute();

    expect(screen.getByText(/login page/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should redirect to home page if user is logged but has no access', () => {
    setUserWithoutAccess();
    renderChatRoute();

    expect(screen.getByText(/home page/i)).toBeInTheDocument();
    expect(screen.queryByText(/protected content/i)).not.toBeInTheDocument();
  });

  it('should render the protected content if the user is logged and has access', () => {
    setUserWithAccess();
    renderChatRoute();

    expect(screen.getByText(/protected content/i)).toBeInTheDocument();
  });

  it('should render protected content while access is loading', () => {
    setLoadingUser();
    renderChatRoute();

    expect(screen.getByText(/protected content/i)).toBeInTheDocument();
    expect(screen.queryByText(/home page/i)).not.toBeInTheDocument();
    expect(screen.queryByText(/login page/i)).not.toBeInTheDocument();
  });
});
