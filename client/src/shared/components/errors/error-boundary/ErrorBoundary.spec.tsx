import { fireEvent, render, screen } from '@testing-library/react';

import ErrorBoundary from './ErrorBoundary.js';

const ProblemChild = () => {
  throw new Error('Boom');
};

describe('ErrorBoundary', () => {
  const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});

  afterAll(() => {
    consoleErrorSpy.mockRestore();
  });

  it('should render children when there is no error', () => {
    render(
      <ErrorBoundary>
        <div>Safe child</div>
      </ErrorBoundary>,
    );

    expect(screen.getByText(/safe child/i)).toBeInTheDocument();
  });

  it('should render default fallback UI when child throws', () => {
    render(
      <ErrorBoundary>
        <ProblemChild />
      </ErrorBoundary>,
    );

    expect(screen.getByText(/something went wrong/i)).toBeInTheDocument();
    expect(
      screen.getByText(/an unexpected error occurred\. please try again\./i),
    ).toBeInTheDocument();

    expect(screen.getByRole('button', { name: /try again/i })).toBeInTheDocument();
  });

  it('should render custom fallback when provided', () => {
    render(
      <ErrorBoundary fallback={<div>Custom fallback</div>}>
        <ProblemChild />
      </ErrorBoundary>,
    );

    expect(screen.getByText(/custom fallback/i)).toBeInTheDocument();
  });

  it('should call onError when child throws', () => {
    const onError = vi.fn();

    render(
      <ErrorBoundary onError={onError}>
        <ProblemChild />
      </ErrorBoundary>,
    );

    expect(onError).toHaveBeenCalledTimes(1);

    const firstCall = onError.mock.calls[0];
    expect(firstCall).toBeDefined();

    if (!firstCall) {
      throw new Error('Expected onError to be called');
    }

    const [errorArg, infoArg] = firstCall;
    expect(errorArg).toBeInstanceOf(Error);
    expect(typeof infoArg).toBe('string');
  });

  it('should call onReset when Try Again button is clicked', async () => {
    const onReset = vi.fn();

    render(
      <ErrorBoundary onReset={onReset}>
        <ProblemChild />
      </ErrorBoundary>,
    );

    const button = screen.getByRole('button', { name: /try again/i });
    await fireEvent.click(button);

    expect(onReset).toHaveBeenCalledTimes(1);
  });
});
