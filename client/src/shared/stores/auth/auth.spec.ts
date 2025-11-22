import { act, renderHook } from '@testing-library/react';

import { useAuth } from './auth.js';

describe('useAuth() Store', () => {
  beforeEach(() => {
    const { result } = renderHook(() => useAuth());

    act(() => {
      result.current.logout();
    });
  });

  it('should expose guest view when no user is set', () => {
    const { result } = renderHook(() => useAuth());

    expect(result.current.isAuthenticated).toBe(false);
    expect(result.current.userId).toBe('');
    expect(result.current.username).toBe('');
    expect(result.current.email).toBe('');
    expect(result.current.token).toBe('');
    expect(result.current.isAdmin).toBe(false);
    expect(result.current.hasProfile).toBe(false);
  });

  it('should set auth state via changeAuthenticationState', () => {
    const user = {
      userId: '123',
      username: 'alice',
      email: 'alice@example.com',
      token: 'test-token',
      isAdmin: true,
      hasProfile: true,
    };

    const { result } = renderHook(() => useAuth());

    act(() => {
      result.current.changeAuthenticationState(user);
    });

    expect(result.current.isAuthenticated).toBe(true);
    expect(result.current.userId).toBe('123');
    expect(result.current.username).toBe('alice');
    expect(result.current.email).toBe('alice@example.com');
    expect(result.current.token).toBe('test-token');
    expect(result.current.isAdmin).toBe(true);
    expect(result.current.hasProfile).toBe(true);
  });

  it('should update hasProfile via changeHasProfileState without changing other fields', () => {
    const user = {
      userId: '321',
      username: 'bob',
      email: 'bob@example.com',
      token: 'another-token',
      isAdmin: false,
      hasProfile: false,
    };

    const { result } = renderHook(() => useAuth());

    act(() => {
      result.current.changeAuthenticationState(user);
    });

    act(() => {
      result.current.changeHasProfileState(true);
    });

    expect(result.current.hasProfile).toBe(true);
    expect(result.current.userId).toBe('321');
    expect(result.current.username).toBe('bob');
    expect(result.current.email).toBe('bob@example.com');
    expect(result.current.token).toBe('another-token');
    expect(result.current.isAdmin).toBe(false);
  });

  it('should reset to guest view on logout', () => {
    const user = {
      userId: '999',
      username: 'charlie',
      email: 'charlie@example.com',
      token: 'secret',
      isAdmin: true,
      hasProfile: true,
    };

    const { result } = renderHook(() => useAuth());

    act(() => {
      result.current.changeAuthenticationState(user);
    });

    act(() => {
      result.current.logout();
    });

    expect(result.current.isAuthenticated).toBe(false);
    expect(result.current.userId).toBe('');
    expect(result.current.username).toBe('');
    expect(result.current.email).toBe('');
    expect(result.current.token).toBe('');
    expect(result.current.isAdmin).toBe(false);
    expect(result.current.hasProfile).toBe(false);
  });
});
