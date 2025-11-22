import { act, renderHook } from '@testing-library/react';
import { vi } from 'vitest';

import { useDebounce } from './useDebounce.js';

describe('useDebounce() hook', () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
    vi.restoreAllMocks();
  });

  it('should return the initial value immediately', () => {
    const { result } = renderHook(() => useDebounce('hello', 500));
    expect(result.current).toBe('hello');
  });

  it('should update the debounced value only after the delay', () => {
    const { result, rerender } = renderHook(({ value }) => useDebounce(value, 500), {
      initialProps: { value: 'first' },
    });

    expect(result.current).toBe('first');

    rerender({ value: 'second' });

    expect(result.current).toBe('first');

    act(() => {
      vi.advanceTimersByTime(500);
    });

    expect(result.current).toBe('second');
  });

  it('should cancel the previous timeout when value changes quickly', () => {
    const { result, rerender } = renderHook(({ value }) => useDebounce(value, 500), {
      initialProps: { value: 'a' },
    });

    expect(result.current).toBe('a');

    rerender({ value: 'b' });
    act(() => {
      vi.advanceTimersByTime(300);
    });

    rerender({ value: 'c' });
    act(() => {
      vi.advanceTimersByTime(300);
    });

    expect(result.current).toBe('a');

    act(() => {
      vi.advanceTimersByTime(200);
    });

    expect(result.current).toBe('c');
  });
});
