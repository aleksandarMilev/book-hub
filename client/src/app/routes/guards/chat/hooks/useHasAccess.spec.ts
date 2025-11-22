import { renderHook, waitFor } from '@testing-library/react';
import { useParams } from 'react-router-dom';
import { type Mock, vi } from 'vitest';

import { useHasAccess } from '@/app/routes/guards/chat/hooks/useHasAccess.js';
import * as api from '@/features/chat/api/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom');
  return {
    ...actual,
    useParams: vi.fn(),
  };
});

vi.mock('@/shared/stores/auth/auth.js', () => ({
  useAuth: vi.fn(),
}));

vi.mock('@/features/chat/api/api.js', () => ({
  hasAccess: vi.fn(),
}));

const mockUseParams = useParams as unknown as Mock;
const mockUseAuth = useAuth as unknown as Mock;
const mockHasAccess = api.hasAccess as unknown as Mock;

describe('useHasAccess Hook', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should not call the API if user is not logged or id/token/userId is missing', async () => {
    mockUseParams.mockReturnValue({ id: undefined });
    mockUseAuth.mockReturnValue({
      token: null,
      isAuthenticated: false,
      userId: null,
    });

    const { result } = renderHook(() => useHasAccess());

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false);
    });

    expect(mockHasAccess).not.toHaveBeenCalled();
    expect(result.current.hasAccess).toBe(false);
    expect(result.current.isAuthenticated).toBe(false);
  });

  it('should call the API and set hasAccess=true when access is granted', async () => {
    mockUseParams.mockReturnValue({ id: '5' });
    mockUseAuth.mockReturnValue({
      token: 'abc123',
      isAuthenticated: true,
      userId: 42,
    });

    mockHasAccess.mockResolvedValue(true);

    const { result } = renderHook(() => useHasAccess());

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false);
    });

    expect(mockHasAccess).toHaveBeenCalledWith(5, 42, 'abc123');
    expect(result.current.hasAccess).toBe(true);
    expect(result.current.isAuthenticated).toBe(true);
  });

  it('should call the API and set hasAccess=false when access is denied', async () => {
    mockUseParams.mockReturnValue({ id: '99' });
    mockUseAuth.mockReturnValue({
      token: 'xyz789',
      isAuthenticated: true,
      userId: 7,
    });

    mockHasAccess.mockResolvedValue(false);

    const { result } = renderHook(() => useHasAccess());

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false);
    });

    expect(mockHasAccess).toHaveBeenCalledWith(99, 7, 'xyz789');
    expect(result.current.hasAccess).toBe(false);
    expect(result.current.isAuthenticated).toBe(true);
  });
});
