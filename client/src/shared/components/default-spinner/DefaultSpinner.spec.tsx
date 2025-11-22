import { render, screen } from '@testing-library/react';

import DefaultSpinner from './DefaultSpinner.js';

describe('Default Spinner Component', () => {
  it('should render the loading text', () => {
    render(<DefaultSpinner />);

    expect(screen.getByText(/loading\.\.\./i)).toBeInTheDocument();
  });

  it('should render the spinner element', () => {
    const { container } = render(<DefaultSpinner />);

    const spinner = container.querySelector('.bookhub-spinner');
    expect(spinner).not.toBeNull();
  });
});
