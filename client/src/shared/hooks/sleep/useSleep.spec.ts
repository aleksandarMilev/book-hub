import { renderHook } from '@testing-library/react';
import { vi } from 'vitest';

import { sleep } from '@/shared/lib/utils/utils.js';

import { useSleep } from './useSleep.js';

vi.mock('@/shared/lib/utils/utils.js', () => ({
  sleep: vi.fn(),
}));

const mockSleep = vi.mocked(sleep);

describe('useSleep() hook', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should call sleep with the provided milliseconds', () => {
    mockSleep.mockResolvedValueOnce(undefined);

    renderHook(() => useSleep(1_000));

    expect(mockSleep).toHaveBeenCalledTimes(1);
    expect(mockSleep).toHaveBeenCalledWith(1_000);
  });

  it('should call sleep with the default value when no argument is provided', () => {
    mockSleep.mockResolvedValueOnce(undefined);

    renderHook(() => useSleep());

    expect(mockSleep).toHaveBeenCalledTimes(1);
    expect(mockSleep).toHaveBeenCalledWith(2_000);
  });
});
