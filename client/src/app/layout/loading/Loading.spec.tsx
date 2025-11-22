import { render, screen } from '@testing-library/react';

import Loading from './Loading.js';

describe('Loading Component', () => {
  it('should render the DefaultSpinner', () => {
    render(<Loading />);

    const spinnerText = screen.getByText(/loading\.\.\./i);
    expect(spinnerText).toBeInTheDocument();
  });

  it('should have a wrapper div with padding of 24px', () => {
    const { container } = render(<Loading />);

    const wrapper = container.firstChild as HTMLElement;
    expect(wrapper).toHaveStyle({ padding: '24px' });
  });
});
