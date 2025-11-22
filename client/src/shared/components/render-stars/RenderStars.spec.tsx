import { render, screen } from '@testing-library/react';

import { RenderStars } from './RenderStars.js';

describe('Render Stars Component', () => {
  it('should display rating with two decimal places', () => {
    render(<RenderStars rating={3.14159} />);

    expect(screen.getByText('3.14')).toBeInTheDocument();
  });

  it('should render correct number of filled and empty stars based on rounded rating', () => {
    const { container } = render(<RenderStars rating={3.2} />);

    const filledStars = container.querySelectorAll('.bh-star.filled');
    const emptyStars = container.querySelectorAll('.bh-star.empty');

    expect(filledStars.length).toBe(3);
    expect(emptyStars.length).toBe(2);
  });

  it('should handle max rating correctly (all filled)', () => {
    const { container } = render(<RenderStars rating={4.6} />);

    const filledStars = container.querySelectorAll('.bh-star.filled');
    const emptyStars = container.querySelectorAll('.bh-star.empty');

    expect(filledStars.length).toBe(5);
    expect(emptyStars.length).toBe(0);
  });

  it('should handle low rating correctly (all empty)', () => {
    const { container } = render(<RenderStars rating={0.4} />);

    const filledStars = container.querySelectorAll('.bh-star.filled');
    const emptyStars = container.querySelectorAll('.bh-star.empty');

    expect(filledStars.length).toBe(0);
    expect(emptyStars.length).toBe(5);
  });
});
