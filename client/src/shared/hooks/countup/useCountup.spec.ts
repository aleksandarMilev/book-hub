import { renderHook, waitFor } from '@testing-library/react';
import { vi } from 'vitest';

import { useCountUp } from './useCountup.js';

describe('useCountUp() hook', () => {
  const originalRAF = window.requestAnimationFrame;
  const originalNow = performance.now;

  afterEach(() => {
    window.requestAnimationFrame = originalRAF;
    performance.now = originalNow;

    vi.restoreAllMocks();
  });

  it('should start from 0', () => {
    window.requestAnimationFrame = vi.fn() as unknown as typeof window.requestAnimationFrame;
    performance.now = vi.fn().mockReturnValue(0);

    const { result } = renderHook(() => useCountUp(100, 1_000));
    expect(result.current).toBe(0);
  });

  it('should eventually reach the end value', async () => {
    let startTime = 0;

    performance.now = vi.fn().mockImplementation(() => startTime);

    window.requestAnimationFrame = ((cb: (time: number) => void) => {
      const now = startTime + 1_000;
      cb(now);

      return 1;
    }) as unknown as typeof window.requestAnimationFrame;

    const { result } = renderHook(() => useCountUp(42, 1_000));

    await waitFor(() => {
      expect(result.current).toBe(42);
    });
  });
});
