import '@testing-library/jest-dom';

import { vi } from 'vitest';

Object.defineProperty(window, 'scrollTo', {
  writable: true,
  value: vi.fn(),
});

