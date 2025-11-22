import { render, screen } from '@testing-library/react';

import MessageDisplay from './Message.js';

describe('Message Display Component', () => {
  it('should render success message with success classes', () => {
    const { container } = render(<MessageDisplay message="All good" isSuccess />);

    expect(screen.getByText(/all good/i)).toBeInTheDocument();

    const root = container.querySelector('.message');
    expect(root).toHaveClass('message');
    expect(root).toHaveClass('success');
    expect(root).not.toHaveClass('error');
  });

  it('should render error message with error classes', () => {
    const { container } = render(<MessageDisplay message="Something failed" isSuccess={false} />);

    expect(screen.getByText(/something failed/i)).toBeInTheDocument();

    const root = container.querySelector('.message');
    expect(root).toHaveClass('message');
    expect(root).toHaveClass('error');
    expect(root).not.toHaveClass('success');
  });
});
