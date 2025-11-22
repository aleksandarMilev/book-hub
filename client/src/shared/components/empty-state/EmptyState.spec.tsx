import { fireEvent, render, screen } from '@testing-library/react';

import EmptyState from './EmptyState.js';

describe('Empty State Component', () => {
  it('should render message and icon', () => {
    render(<EmptyState message="No books found" icon={<span data-testid="empty-icon" />} />);

    expect(screen.getByText(/no books found/i)).toBeInTheDocument();
    expect(screen.getByTestId('empty-icon')).toBeInTheDocument();
  });

  it('renders title when provided', () => {
    render(<EmptyState title="Nothing here" message="No content yet" icon={<span />} />);

    expect(screen.getByText(/nothing here/i)).toBeInTheDocument();
  });

  it('should not render title when not provided', () => {
    render(<EmptyState message="No content yet" icon={<span />} />);

    expect(screen.queryByRole('heading')).not.toBeInTheDocument();
  });

  it('should render action button and calls onAction when both provided', async () => {
    const onAction = vi.fn();

    render(
      <EmptyState
        message="No content yet"
        icon={<span />}
        actionLabel="Create one"
        onAction={onAction}
      />,
    );

    const button = screen.getByRole('button', { name: /create one/i });
    expect(button).toBeInTheDocument();

    fireEvent.click(button);

    expect(onAction).toHaveBeenCalledTimes(1);
  });

  it('should not render action button if actionLabel is missing', () => {
    const onAction = vi.fn();

    render(<EmptyState message="No content yet" icon={<span />} onAction={onAction} />);

    expect(screen.queryByRole('button')).not.toBeInTheDocument();
  });

  it('should not render action button if onAction is missing', () => {
    render(<EmptyState message="No content yet" icon={<span />} actionLabel="Click me" />);

    expect(screen.queryByRole('button')).not.toBeInTheDocument();
  });
});
