import { render, screen } from '@testing-library/react';
import { HttpStatusCode } from 'axios';
import { vi } from 'vitest';

import { routes } from '@/shared/lib/constants/api.js';

import { ErrorRedirect } from './ErrorsRedirect.js';

vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom');
  return {
    ...actual,
    Navigate: ({ to, state }: { to: string; state?: { message?: string } }) => (
      <div data-testid="navigate" data-to={to} data-message={state?.message ?? ''} />
    ),
  };
});

const makeError = (status: number, message: string, name = 'Error') =>
  ({ status, message, name }) as { status: number; message: string; name: string };

describe('Error Redirect Component', () => {
  it('should redirect unauthorized/forbidden to access denied', () => {
    const error = makeError(HttpStatusCode.Unauthorized, 'No access');

    render(<ErrorRedirect error={error} />);

    const navigation = screen.getByTestId('navigate');
    expect(navigation).toHaveAttribute('data-to', routes.accessDenied);
    expect(navigation).toHaveAttribute('data-message', 'No access');
  });

  it('redirects not found to not found route', () => {
    const error = makeError(HttpStatusCode.NotFound, 'Missing');

    render(<ErrorRedirect error={error} />);

    const navigation = screen.getByTestId('navigate');
    expect(navigation).toHaveAttribute('data-to', routes.notFound);
    expect(navigation).toHaveAttribute('data-message', 'Missing');
  });

  it('redirects other errors to bad request', () => {
    const error = makeError(HttpStatusCode.BadRequest, 'Bad');

    render(<ErrorRedirect error={error} />);

    const navigation = screen.getByTestId('navigate');
    expect(navigation).toHaveAttribute('data-to', routes.badRequest);
    expect(navigation).toHaveAttribute('data-message', 'Bad');
  });
});
