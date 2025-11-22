import { act, renderHook } from '@testing-library/react';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

import { useMessage } from './message.js';

describe('useMessage Store', () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.clearAllTimers();
    vi.useRealTimers();
  });

  it('should expose initial empty state', () => {
    const { result } = renderHook(() => useMessage());

    expect(result.current.isShowing).toBe(false);
    expect(result.current.message).toBeNull();
    expect(result.current.isSuccess).toBe(true);
  });

  it('should show a success message by default', () => {
    const { result } = renderHook(() => useMessage());

    act(() => {
      result.current.showMessage('Operation completed');
    });

    expect(result.current.isShowing).toBe(true);
    expect(result.current.message).toBe('Operation completed');
    expect(result.current.isSuccess).toBe(true);
  });

  it('should show an error message when isSuccess is false', () => {
    const { result } = renderHook(() => useMessage());

    act(() => {
      result.current.showMessage('Something went wrong', false);
    });

    expect(result.current.isShowing).toBe(true);
    expect(result.current.message).toBe('Something went wrong');
    expect(result.current.isSuccess).toBe(false);
  });

  it('should auto-hide the message after the given duration', () => {
    const { result } = renderHook(() => useMessage());

    act(() => {
      result.current.showMessage('Temporary message', true, 1_000);
    });

    expect(result.current.isShowing).toBe(true);
    expect(result.current.message).toBe('Temporary message');

    act(() => {
      vi.advanceTimersByTime(1_000);
    });

    expect(result.current.isShowing).toBe(false);
    expect(result.current.message).toBeNull();
  });

  it('should cancel previous hide timer when showMessage is called again', () => {
    const { result } = renderHook(() => useMessage());

    act(() => {
      result.current.showMessage('First message', true, 1_000);
    });

    act(() => {
      vi.advanceTimersByTime(500);
      result.current.showMessage('Second message', true, 1_000);
    });

    act(() => {
      vi.advanceTimersByTime(600);
    });

    expect(result.current.isShowing).toBe(true);
    expect(result.current.message).toBe('Second message');
  });

  it('should clear the message and cancel the timer when clearMessage is called', () => {
    const { result } = renderHook(() => useMessage());

    act(() => {
      result.current.showMessage('Will be cleared', true, 1_000);
    });

    act(() => {
      result.current.clearMessage();
    });

    expect(result.current.isShowing).toBe(false);
    expect(result.current.message).toBeNull();

    act(() => {
      vi.advanceTimersByTime(1_000);
    });

    expect(result.current.isShowing).toBe(false);
    expect(result.current.message).toBeNull();
  });
});
