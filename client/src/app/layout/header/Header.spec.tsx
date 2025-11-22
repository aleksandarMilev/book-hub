import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { type Mock, vi } from 'vitest';

import Header from '@/app/layout/header/Header.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

vi.mock('@/shared/stores/auth/auth.js', () => ({
  useAuth: vi.fn(),
}));

vi.mock('@/features/notification/components/last-list/LastNotifications.js', () => ({
  default: () => <div data-testid="last-notifications" />,
}));

const mockUseAuth = useAuth as unknown as Mock;

const renderHeader = () => {
  render(
    <MemoryRouter>
      <Header />
    </MemoryRouter>,
  );
};

const baseLinks = [
  'home',
  'books',
  'authors',
  'articles',
  'chats',
  'users',
  'create book',
  'create author',
] as const;

describe('Header Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('For Guest User', () => {
    const setGuest = () => {
      mockUseAuth.mockReturnValue({
        isAuthenticated: false,
        isAdmin: false,
        username: null,
      });
    };

    it('should render the allowed links', () => {
      setGuest();
      renderHeader();

      const allowed = [...baseLinks, 'create chat', 'login', 'register'] as const;
      for (const link of allowed) {
        expect(screen.getByRole('link', { name: new RegExp(link, 'i') })).toBeInTheDocument();
      }
    });

    it('should not render the forbidden links', () => {
      setGuest();
      renderHeader();

      const forbidden = ['logout', 'my profile'] as const;
      for (const link of forbidden) {
        expect(screen.queryByRole('link', { name: new RegExp(link, 'i') })).not.toBeInTheDocument();
      }
    });

    it('should not render the last notifications component', () => {
      setGuest();
      renderHeader();

      expect(screen.queryByTestId('last-notifications')).not.toBeInTheDocument();
    });
  });

  describe('For Authenticated User', () => {
    const setUser = () => {
      mockUseAuth.mockReturnValue({
        isAuthenticated: true,
        isAdmin: false,
        username: '6a6ko',
      });
    };

    it('should render the allowed links', () => {
      setUser();
      renderHeader();

      const allowed = [...baseLinks, 'create chat', 'logout', 'my profile'] as const;
      for (const link of allowed) {
        expect(screen.getByRole('link', { name: new RegExp(link, 'i') })).toBeInTheDocument();
      }
    });

    it('should not render the forbidden links', () => {
      setUser();
      renderHeader();

      const forbidden = ['login', 'register', 'create article'] as const;
      for (const link of forbidden) {
        expect(screen.queryByRole('link', { name: new RegExp(link, 'i') })).not.toBeInTheDocument();
      }
    });

    it('should render the last notifications component', () => {
      setUser();
      renderHeader();

      expect(screen.queryByTestId('last-notifications')).toBeInTheDocument();
    });

    it('should render a greeting', () => {
      setUser();
      renderHeader();

      expect(screen.getByText(/hello, 6a6ko/i)).toBeInTheDocument();
    });
  });

  describe('For Admin User', () => {
    const setAdmin = () => {
      mockUseAuth.mockReturnValue({
        isAuthenticated: true,
        isAdmin: true,
        username: 'admin',
      });
    };

    it('should render the allowed links', () => {
      setAdmin();
      renderHeader();

      const allowed = [...baseLinks, 'create article', 'logout'] as const;
      for (const link of allowed) {
        expect(screen.getByRole('link', { name: new RegExp(link, 'i') })).toBeInTheDocument();
      }
    });

    it('should not render the forbidden links', () => {
      setAdmin();

      const forbidden = ['login', 'register', 'create chat', 'my profile'] as const;
      for (const link of forbidden) {
        expect(screen.queryByRole('link', { name: new RegExp(link, 'i') })).not.toBeInTheDocument();
      }
    });

    it('should render the last notifications component', () => {
      setAdmin();
      renderHeader();

      expect(screen.queryByTestId('last-notifications')).toBeInTheDocument();
    });

    it('should render a greeting', () => {
      setAdmin();
      renderHeader();

      expect(screen.getByText(/hello, admin/i)).toBeInTheDocument();
    });
  });
});
