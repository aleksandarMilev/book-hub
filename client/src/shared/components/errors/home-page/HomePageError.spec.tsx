import { fireEvent, render, screen } from '@testing-library/react';

import HomePageError from './HomePageError.js';

describe('Home Page Error Component', () => {
  it('should render the error message', () => {
    render(<HomePageError message="Something went wrong" />);

    expect(screen.getByText(/something went wrong/i)).toBeInTheDocument();
  });

  it('should render retry button and calls onRetry when provided', async () => {
    const onRetry = vi.fn();

    render(<HomePageError message="Error" onRetry={onRetry} />);

    const button = screen.getByRole('button', { name: /try again/i });
    expect(button).toBeInTheDocument();

    fireEvent.click(button);

    expect(onRetry).toHaveBeenCalledTimes(1);
  });

  it('should not render retry button when onRetry is not provided', () => {
    render(<HomePageError message="Error" />);

    expect(screen.queryByRole('button', { name: /try again/i })).not.toBeInTheDocument();
  });
});
